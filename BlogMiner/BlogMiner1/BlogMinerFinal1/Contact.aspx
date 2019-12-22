<%@ Page Language="C#" MasterPageFile="~/design/MasterPage.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="_Default" Title="Untitled Page" %>
<%@ Import Namespace="Artisteer" %>
<%@ Register TagPrefix="artisteer" Namespace="Artisteer" %>
<%@ Register TagPrefix="art" TagName="DefaultMenu" Src="DefaultMenu.ascx" %> 
<%@ Register TagPrefix="art" TagName="DefaultHeader" Src="DefaultHeader.ascx" %> 
<%@ Register TagPrefix="art" TagName="DefaultSidebar1" Src="DefaultSidebar1.ascx" %>
          <%@ Register TagPrefix="art" TagName="DefaultSidebar2" Src="DefaultSidebar2.ascx" %>
          

<asp:Content ID="PageTitle" ContentPlaceHolderID="TitleContentPlaceHolder" Runat="Server">
    BlogMiner!
</asp:Content>

<asp:Content ID="MenuContent" ContentPlaceHolderID="MenuContentPlaceHolder" Runat="Server">
    <art:DefaultMenu ID="DefaultMenuContent" runat="server" />
</asp:Content>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContentPlaceHolder" Runat="Server">
    <art:DefaultHeader ID="DefaultHeader" runat="server" />
</asp:Content>
<asp:Content ID="SideBar1" ContentPlaceHolderID="Sidebar1ContentPlaceHolder" Runat="Server">
    <art:DefaultSidebar1 ID="DefaultSidebar1Content" runat="server" />
</asp:Content>
<asp:Content ID="SideBar2" ContentPlaceHolderID="Sidebar2ContentPlaceHolder" Runat="Server">
    <art:DefaultSidebar2 ID="DefaultSidebar2Content" runat="server" />
</asp:Content>

<asp:Content ID="SheetContent" ContentPlaceHolderID="SheetContentPlaceHolder" Runat="Server">

<artisteer:Article ID="Article1" Caption="Welcome! :)" runat="server"><ContentTemplate>
    <img src="images/BlogMiner.png" title="BlogMiner!" alt="Main image!" style="float:left;" />
    <img src="images/VS.png" title="Visual Studio Logo" alt="Visual Studio Logo" style="float:left;border:0;margin: 1em 1em 0 0;" />
    <p><h1>BlogMinig Application for Movie Review Classifcation !</h1>
    <%--<a href="javascript:void(0)" title="link">link</a>, <a class="visited" href="javascript:void(0)" title="visited link">
        visited link</a>,
     <a class="hover" href="javascript:void(0)" title="hovered link">hovered link</a> --%>
     <br />
        With increasing usage of the world's biggest library: The INTERNET and social networking, blogging and blog pages are growing rapidly. Blog pages have become the most popular means to express one's opinions. People share their ideas, opinions, interests, emotions, experiences and knowledge with other via internet every day.</p>
        <br />
        <p>Our application grabs the comments and movie reviews from well known sites and parses it in a manageable, easy to use format. The parsed data is then analyzed i.e it is termed as sentiment analysis; the sentiments expressed by the commenters and reviewers is analyzed and studied carefully: correcting its spelling, determining subjectivity and the sentiment orientation in terms of positive and negative scores.</p>
        <br />
        <p>The scores are then summed up to get the final sentiment score for the whole movie as well as for the individual categories such as director, actor/actress etc</p>
    <p>
    	<span class="art-button-wrapper">
    		<span class="art-button-l"> </span>
    		<span class="art-button-r"> </span>
    		<a class="art-button" href="KnowMore.aspx">Know technically more!...</a>
    	</span>
    </p>
    <div class="cleared"></div>
    <div class="art-content-layout overview-table">
    	<div class="art-content-layout-row">
    		<div class="art-layout-cell">
          <div class="overview-table-inner">
    	      <h4>Support/Contact</h4>
    						  <img src="images/01.png" width="55" height="55" alt="an image" class="image" />
    						  <p><a href="Home.aspx">Click here to contact us!</a></p>
           </div>
    		</div><!-- end cell -->
    		<%--<div class="art-layout-cell">
        <div class="overview-table-inner">
    		  <h4>Development</h4>
    						  <img src="images/02.png" width="55" height="55" alt="an image" class="image" />
    						  <p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Quisque sed felis. 
                                  Aliquam sit amet felis. Mauris semper, velit semper laoreet dictum, quam diam 
                                  dictum urna. </p>
    				</div>
    		</div>--%><!-- end cell -->
    		<div class="art-layout-cell">
        <div class="overview-table-inner">
    		  <h4>Development Strategy: How it works! :)    </h4>
    
    						  <img src="images/03.png" width="55" height="55" alt="an image" class="image" />
    						  <p><a href="KnowMore.aspx" >Click here to know more!</a></p>
                  </div>
    		</div><!-- end cell -->
    	</div><!-- end row -->
    </div><!-- end table -->
           </ContentTemplate></artisteer:Article>
    <%--<artisteer:Article ID="Article2" Caption="Text, &lt;a href=&quot;#&quot; rel=&quot;bookmark&quot; title=&quot;Permanent Link to this Post&quot;&gt;Link&lt;/a&gt;, &lt;a class=&quot;visited&quot; href=&quot;#&quot; rel=&quot;bookmark&quot; title=&quot;Visited Hyperlink&quot;&gt;Visited&lt;/a&gt;, &lt;a class=&quot;hovered&quot; href=&quot;#&quot; rel=&quot;bookmark&quot; title=&quot;Hovered Hyperlink&quot;&gt;Hovered&lt;/a&gt;" runat="server"><ContentTemplate><p>
        Lorem <sup>superscript</sup> dolor <sub>subscript</sub> amet, consectetuer 
        adipiscing elit, <a href="#" title="test link">test link</a>. Nullam dignissim 
        convallis est. Quisque aliquam. <cite>cite</cite>. Nunc iaculis suscipit dui. 
        Nam sit amet sem. Aliquam libero nisi, imperdiet at, tincidunt nec, gravida 
        vehicula, nisl. Praesent mattis, massa quis luctus fermentum, turpis mi volutpat 
        justo, eu volutpat enim diam eget metus. Maecenas ornare tortor. Donec sed 
        tellus eget sapien fringilla nonummy. <acronym title="National Basketball Association">
        NBA</acronym> Mauris a ante. Suspendisse quam sem, consequat at, commodo vitae, 
        feugiat in, nunc. Morbi imperdiet augue quis tellus.  <abbr title="Avenue">AVE</abbr></p>
    
      <h1>Heading 1</h1>
      <h2>Heading 2</h2>
      <h3>Heading 3</h3>
      <h4>Heading 4</h4>
      <h5>Heading 5</h5>
      <h6>Heading 6</h6>
    
      <blockquote>
            <p>
                “This stylesheet is going to help so freaking much.”
                <br />
                -Blockquote
            </p>
        </blockquote>
    
        <br />
    
        <table class="art-article" border="0" cellspacing="0" cellpadding="0">
      <tbody>
        <tr>
          <th>Header</th>
          <th>Header</th>
          <th>Header</th>
        </tr>
        <tr>
          <td>Data</td>
          <td>Data</td>
          <td>Data</td>
        </tr>
        <tr class="even">
          <td>Data</td>
          <td>Data</td>
          <td>Data</td>
        </tr>
        <tr>
          <td>Data</td>
          <td>Data</td>
          <td>Data</td>
        </tr>
      </tbody></table>
    
    	<p>
    		<span class="art-button-wrapper">
    			<span class="art-button-l"> </span>
    			<span class="art-button-r"> </span>
    			<a class="art-button" href="javascript:void(0)">Join&nbsp;Now!</a>
    		</span>
    	</p>
           </ContentTemplate></artisteer:Article>--%>
    

</asp:Content>
