namespace MGL.SharePoint.Membership
{
    public static class Helper
    {
        public static string NormalizeUserName(string loginName, bool keepDomain)
        {
            string userName;
            var pipePos = loginName.IndexOf('|');
            if (pipePos != -1)
            {
                var parts = loginName.Split(new char[] { '|' });
                userName = parts[1];
            }
            else
            {
                userName = loginName;
            }
            if (!keepDomain)
            {
                var backslashPos = userName.IndexOf('\\');
                if (backslashPos != -1)
                {
                    var parts = userName.Split(new char[] { '\\' });
                    userName = parts[1];
                }
            }
            return userName;
        }

        public static bool IsClaimsUser(string loginName)
        {
            return loginName.IndexOf('|') != -1;
        }

        public static bool IsUserWithDomain(string loginName)
        {
            return loginName.IndexOf('\\') != -1;
        }
    }
}
