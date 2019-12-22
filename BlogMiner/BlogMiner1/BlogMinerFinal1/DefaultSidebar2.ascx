<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DefaultSidebar2.ascx.cs" Inherits="Sidebar2" %>
<%@ Import Namespace="Artisteer" %>
<%@ Register TagPrefix="artisteer" Namespace="Artisteer" %>
<artisteer:Block ID="ContactInformationBlock" Caption="Contact Info" runat="server"><ContentTemplate><div>
                <img src="images/contact.jpg" alt="an image" style="margin: 0 auto;display:block;width:95%" />
          <br />
          <b>Company Co.</b><br />
          Las Vegas, NV 12345<br />
          Email: <a href="mailto:info@company.com">info@company.com</a><br />
          <br />
          Phone: (123) 456-7890 <br />
          Fax: (123) 456-7890
          </div>
             </ContentTemplate></artisteer:Block>
