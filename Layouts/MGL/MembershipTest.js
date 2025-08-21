(function (window) {

    "use strict"

    const operations = [
        {
            name: "GetAllSiteGroups",
            parameters: [],
            returnType: "stringList"
        },
        {
            name: "GetSiteGroupMembers",
            parameters: ["group"],
            returnType: "stringList"
        },
        {
            name: "IsUserInSiteGroup",
            parameters: [
                "login",
                "group"
            ],
            returnType: "bool"
        },
        {
            name: "IsUserInADGroup",
            parameters: [
                "login",
                "group"
            ],
            returnType: "bool"
        },
        {
            name: "GetADGroupsOfUser",
            parameters: [
                "login"
            ],
            returnType: "stringList"
        },
        {
            name: "GetADGroupMembers",
            parameters: ["group"],
            returnType: "stringList"
        },
        {
            name: "GetSiteGroupsOfUser",
            parameters: [
                "login"
            ],
            returnType: "stringList"
        },
        {
            name: "ResolveADGroup",
            parameters: [
                "group"
            ],
            returnType: "adGroup"
        }
    ];

    var endpointUrl;
    var btnSubmit;
    var spanUrl;
    var selOperation;
    var spanOutput;

    function init() {
        initSiteInfo();
        selOperation = document.getElementById("selOperation");
        selOperation.addEventListener("change", selectionChanged);
        btnSubmit = document.getElementById("btnSubmit");
        btnSubmit.addEventListener("click", send);
        spanUrl = document.getElementById("spanUrl");
        spanOutput = document.getElementById("output");
        fillSelect();
    }

    async function send() {
        clearOutput();
        const operationName = selOperation.value;
        if (operationName) {
            const operation = operations.filter(o => o.name === operationName)[0];

            const result = await fetch(endpointUrl, {
                method: 'GET'
            });

            if (result.ok) {
                const value = await result.json();
                if (value.Success) {
                    if (operation.returnType === 'stringList') {
                        if (value.Result.length === 0) {
                            spanOutput.innerHTML = "Es wurden keine Werte zurückgeliefert."
                        } else {
                            const ul = document.createElement("ul");
                            value.Result.forEach((s) => {
                                ul.insertAdjacentHTML("beforeend", `<li>${s}`)
                            });
                            spanOutput.insertAdjacentElement("beforeend", ul);
                        }
                    } else {
                        spanOutput.innerHTML = "Ergebnis: " + value.Result.toString();
                    }
                } else {
                    showError(value.Error);
                }
                const prettyJSON = syntaxHighlight(JSON.stringify(value, undefined, 4));
                spanOutput.insertAdjacentHTML("beforeend", `<pre>${prettyJSON}</pre>`)
            } else {
                if (result.status === 404) {
                    showError("Fehler: der Endpunkt wurde nicht gefunden.")
                } else {
                    showError(`Fehler: StatusCode ${result.status}, Fehlertext: ${result.statusText}`);
                }
            }
        }
    }

    function showError(text) {
        spanOutput.innerHTML = `<span style='color: red'>${text}</span>`;
    }
    
    function selectionChanged() {
        clearInputRegion();
        clearOutput();
        operations.forEach(o => {
            if (o.name === this.value) {
                o.parameters.forEach(name => { addInput(name) });
            }
        });
        updateUrl();
    }

    function clearInputRegion() {
        const region = document.getElementById("parms");
        region.querySelectorAll("input").forEach(input => input.removeEventListener('input', updateUrl));
        region.querySelectorAll("input").forEach(input => input.removeEventListener('keydown', keydown));
        region.innerHTML = "";
    }

    function clearOutput() {
        spanOutput.innerHTML = "";
    }

    function addInput(name) {
        document.getElementById("parms").insertAdjacentHTML('beforeend', `<label for=inp${name}>${name}</label><input id='inp${name}'><br>`);
        document.getElementById(`inp${name}`).addEventListener('input', updateUrl);
        document.getElementById(`inp${name}`).addEventListener('keydown', keydown);
    }

    function keydown(ev) {
        if (ev.keyCode === 13) {
            btnSubmit.click();
        }
    }

    function updateUrl() {
        const operationName = document.getElementById('selOperation').value;
        if (operationName) {
            const operation = operations.filter(o => o.name === operationName)[0];
            let url = `${getWebUrl()}_vti_bin/MGL/Membership.svc/${operationName}`;
            let delim = '?';
            operation.parameters.forEach(name => {
                const inp = document.getElementById(`inp${name}`);
                if (inp) {
                    url += `${delim}${name}=${encodeURIComponent(inp.value)}`;
                    delim = '&';
                }
            });

            spanUrl.innerText = url;
            endpointUrl = url;
            btnSubmit.disabled = false;
        }
        else {
            spanUrl.innerText = '';
            endpointUrl = '';
            btnSubmit.disabled = true;
        }
    }

    function getWebUrl() {
        const url = window.location.href.toLocaleLowerCase();
        const pos = url.indexOf('_layouts');
        return url.substring(0, pos);
    }

    function fillSelect() {
        operations.forEach(o => {
            selOperation.insertAdjacentHTML('beforeend', `<option>${o.name}</option>`)
        });
    }

    /// Kudos to https://stackoverflow.com/users/27862/user123444555621
    function syntaxHighlight(json) {
        json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
        return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, function (match) {
            var cls = 'number';
            if (/^"/.test(match)) {
                if (/:$/.test(match)) {
                    cls = 'key';
                } else {
                    cls = 'string';
                }
            } else if (/true|false/.test(match)) {
                cls = 'boolean';
            } else if (/null/.test(match)) {
                cls = 'null';
            }
            return '<span class="' + cls + '">' + match + '</span>';
        });
    }

    // <p>Sie befinden sich hier: <span id="spanSite"></span> [<a id="aSiteSettings" href="#" target="_blank">Websiteeinstellungen</a>]</p>
    function initSiteInfo() {
        const siteUrl = getWebUrl();
        document.querySelector('#spanSite').innerHTML = siteUrl;
        document.querySelector("#aSiteSettings").href = `${siteUrl}_layouts/settings.aspx`;
    }

    window.addEventListener("DOMContentLoaded", init);
})(window);