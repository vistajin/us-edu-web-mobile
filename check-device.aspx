<%-- Updated by Vista --%>
<%
    string strUserAgent = Request.UserAgent.ToString().ToLower();
    if (strUserAgent != null)
    {
        if (Request.Browser.IsMobileDevice == true || strUserAgent.Contains("iphone") ||
        strUserAgent.Contains("blackberry") || strUserAgent.Contains("mobile") ||
        strUserAgent.Contains("windows ce") || strUserAgent.Contains("opera mini") ||
        strUserAgent.Contains("palm"))
        {
            Response.Redirect("mobile.aspx?pageId=" + Request.QueryString["pageId"]);
        }
    }
%>
