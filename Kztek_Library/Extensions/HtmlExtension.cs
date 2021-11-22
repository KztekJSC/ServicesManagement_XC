using System;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Kztek_Library.Extensions
{
    public static class HtmlExtension
    {
        public static IHtmlContent GeneratePagingFooter(this IHtmlHelper htmlHelper, int totalPage, int currentPage, int itemsPerPageingFooter, string cssClass, Func<int, string> pageUrl)
        {
            var sb = new StringBuilder();
            sb.Append("<ul class='pagination' style='float:right'>");
            if (currentPage == 1)
            {
                sb.Append("<li class='paginate_button previous disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_first'><a href='javascript:void(0);' rel='nofollow'>First</a></li>");
                sb.Append("<li class='paginate_button previous disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_previous'><a href='javascript:void(0);' rel='nofollow'>Previous</a></li>");
            }
            else
            {
                sb.Append("<li class='paginate_button previous' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_first'><a href='" + pageUrl(1) + "'>First</a></li>");
                sb.Append("<li class='paginate_button previous' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_previous'><a href='" + pageUrl(currentPage - 1) + "'>Previous</a></li>");
            }

            const int pageHold = 10;
            var totalHold = totalPage / pageHold + 1;
            var currentHold = currentPage / pageHold >= 1 && currentPage % pageHold >= 1 ?
                currentPage / pageHold + 1 : currentPage / pageHold;
            currentHold = currentHold == 0 ? 1 : currentHold;

            var pointStart = 1;

            if (currentPage / pageHold >= 1 && currentPage % pageHold >= 1)
                pointStart = currentPage / pageHold * pageHold + 1;
            else if (currentPage / pageHold > 0)
                pointStart = (currentPage / pageHold - 1) * pageHold + 1;

            if (currentHold == 1)
            {
                //sb.Append("<div class='t-numeric'>");
                for (var i = pointStart; i <= ((totalPage < pageHold) ? totalPage : pointStart + pageHold - 1); i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li class='paginate_button active' aria-controls='dynamic-table' tabindex='0'><a href='javascript:void(0);'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(i) + "'>{0}</a></li>", i);
                    }
                }
                if (totalHold > 1)
                    sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * currentHold + 1) + "'>...</a></li>");
                //sb.Append("</div>");
            }
            else if (currentHold == totalHold)
            {
                //sb.Append("<div class='t-numeric'>");
                sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * (currentHold - 1)) + "'>...</a></li>");
                for (var i = pointStart; i <= totalPage; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li class='paginate_button active' aria-controls='dynamic-table' tabindex='0'><a href='javascript:void(0);'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(i) + "'>{0}</a></li>", i);
                    }
                }
                //sb.Append("</div>");
            }
            else
            {
                //sb.Append("<div class='t-numeric'>");
                sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * (currentHold - 1)) + "'>...</a></li>");
                for (var i = pointStart; i <= pointStart + pageHold - 1; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li class='paginate_button active' aria-controls='dynamic-table' tabindex='0'><a href='javascript:void(0);'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(i) + "'>{0}</a></li>", i);
                    }
                }
                sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * currentHold + 1) + "'>...</a></li>");
                //sb.Append("</div>");
            }

            if (currentPage == totalPage)
            {
                sb.Append("<li class='paginate_button next disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_next'><a href='javascript:void(0);' rel='nofollow'>Next</a></li>");
                sb.Append("<li class='paginate_button next disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_last'><a href='javascript:void(0);' rel='nofollow'>Last</a></li>");
            }
            else
            {
                sb.Append("<li class='paginate_button next' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_next'><a href='" + pageUrl(currentPage + 1) + "'>Next</a></li>");
                sb.Append("<li class='paginate_button next' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_last'><a href='" + pageUrl(totalPage) + "'>Last</a></li");
            }

            sb.Append("</ul>");
            //sb.Append("</div>");
            //sb.Append("<div class='t-pager-size'><div class='t-pager-size-chosen'>10</div><ul>" +
            //          "<li>10</li>" +
            //          "<li>15</li>" +
            //          "<li>20</li>" +
            //          "<li>50</li>" +
            //          "<li>100</li>" +
            //          "</ul>" +
            //          "<div class='sprite t-icon-arrow-bottom'></div></div>");
            //sb.Append("</div>");

            return new HtmlString(sb.ToString());
        }

        public static IHtmlContent GeneratePagingFooterAjax(this IHtmlHelper htmlHelper, int totalPage, int totalItem, int currentPage, int itemsPerPageingFooter, string cssClass, Func<int, string> pageUrl)
        {
            var sb = new StringBuilder();
            sb.Append("<ul class='pagination' style='float:right'>");
            if (currentPage == 1)
            {
                sb.Append("<li class='paginate_button previous disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_first'><a href='javascript:void(0);' rel='nofollow'><i class='fa fa-step-backward'></i></a></li>");
                sb.Append("<li class='paginate_button previous disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_previous'><a href='javascript:void(0);' rel='nofollow'><i class='fa fa-caret-left'></i></a></li>");
            }
            else
            {
                sb.Append("<li class='paginate_button previous' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_first'><a href='" + pageUrl(1) + "' idata='1'><i class='fa fa-step-backward'></i></a></li>");
                sb.Append("<li class='paginate_button previous' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_previous'><a href='" + pageUrl(currentPage - 1) + "' idata='" + (currentPage - 1) + "'><i class='fa fa-caret-left'></i></a></li>");
            }

            const int pageHold = 10;
            var totalHold = totalPage / pageHold + 1;
            var currentHold = currentPage / pageHold >= 1 && currentPage % pageHold >= 1 ?
                currentPage / pageHold + 1 : currentPage / pageHold;
            currentHold = currentHold == 0 ? 1 : currentHold;

            var pointStart = 1;

            if (currentPage / pageHold >= 1 && currentPage % pageHold >= 1)
                pointStart = currentPage / pageHold * pageHold + 1;
            else if (currentPage / pageHold > 0)
                pointStart = (currentPage / pageHold - 1) * pageHold + 1;

            if (currentHold == 1)
            {
                //sb.Append("<div class='t-numeric'>");
                for (var i = pointStart; i <= ((totalPage < pageHold) ? totalPage : pointStart + pageHold - 1); i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li class='paginate_button active' aria-controls='dynamic-table' tabindex='0'><a href='javascript:void(0);'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(i) + "' idata='" + i + "'>{0}</a></li>", i);
                    }
                }
                if (totalHold > 1)
                    sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * currentHold + 1) + "' idata='" + (pageHold * currentHold + 1) + "'>...</a></li>");
                //sb.Append("</div>");
            }
            else if (currentHold == totalHold)
            {
                //sb.Append("<div class='t-numeric'>");
                sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * (currentHold - 1)) + "' idata='" + (pageHold * (currentHold - 1)) + "'>...</a></li>");
                for (var i = pointStart; i <= totalPage; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li class='paginate_button active' aria-controls='dynamic-table' tabindex='0'><a href='javascript:void(0);'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(i) + "' idata='" + i + "'>{0}</a></li>", i);
                    }
                }
                //sb.Append("</div>");
            }
            else
            {
                //sb.Append("<div class='t-numeric'>");
                sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * (currentHold - 1)) + "' idata='" + (pageHold * (currentHold - 1)) + "'>...</a></li>");
                for (var i = pointStart; i <= pointStart + pageHold - 1; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li class='paginate_button active' aria-controls='dynamic-table' tabindex='0'><a href='javascript:void(0);'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(i) + "' idata='" + i + "'>{0}</a></li>", i);
                    }
                }
                sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * currentHold + 1) + "' idata='" + (pageHold * currentHold + 1) + "'>...</a></li>");
                //sb.Append("</div>");
            }

            if (currentPage == totalPage)
            {
                sb.Append("<li class='paginate_button next disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_next'><a href='javascript:void(0);' rel='nofollow'><i class='fa fa-caret-right'></i></a></li>");
                sb.Append("<li class='paginate_button next disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_last'><a href='javascript:void(0);' rel='nofollow'><i class='fa fa-step-forward'></i></a></li>");
            }
            else
            {
                sb.Append("<li class='paginate_button next' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_next'><a href='" + pageUrl(currentPage + 1) + "' idata='" + (currentPage + 1) + "'><i class='fa fa-caret-right'></i></a></li>");
                sb.Append("<li class='paginate_button next' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_last'><a href='" + pageUrl(totalPage) + "' idata='" + totalPage + "'><i class='fa fa-step-forward'></i></a></li>");
            }
            //Chọn số bản ghi cần hiển thị

            sb.Append("<li class=\"paginate_button next\" aria-controls=\"dynamic-table\" tabindex=\"0\" id=\"dynamic-table_last\">");
            //sb.Append("<select class=\"form-control cssPagingSelect\">");
            //sb.Append(string.Format("<option value=\"10\" {0}>10</option>", itemsPerPageingFooter == 10 ? "selected='selected'" : ""));
            //sb.Append(string.Format("<option value=\"20\" {0}>20</option>", itemsPerPageingFooter == 20 ? "selected='selected'" : ""));
            //sb.Append(string.Format("<option value=\"30\" {0}>30</option>", itemsPerPageingFooter == 30 ? "selected='selected'" : ""));
            //sb.Append(string.Format("<option value=\"50\" {0}>50</option>", itemsPerPageingFooter == 50 ? "selected='selected'" : ""));
            //sb.Append(string.Format("<option value=\"100\" {0}>100</option>", itemsPerPageingFooter == 100 ? "selected='selected'" : ""));
            //sb.Append("</select>");
            sb.Append("</li>");
            sb.Append("<li class=\"paginate_button next\" aria-controls=\"dynamic-table\" tabindex=\"0\" >");
            //sb.Append("<p style=\"line-height:30px;margin-left: 10px;color: #777;\">");
            //sb.Append("/ <b>" + totalItem + "</b> bản ghi");
            //sb.Append("</p>");
            sb.Append("</li>");

            sb.Append("</ul>");
            //sb.Append("</div>");
            //sb.Append("<div class='t-pager-size'><div class='t-pager-size-chosen'>10</div><ul>" +
            //          "<li>10</li>" +
            //          "<li>15</li>" +
            //          "<li>20</li>" +
            //          "<li>50</li>" +
            //          "<li>100</li>" +
            //          "</ul>" +
            //          "<div class='sprite t-icon-arrow-bottom'></div></div>");
            //sb.Append("</div>");

            return new HtmlString(sb.ToString());
        }
    }
}