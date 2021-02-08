<%@ Page Title="EasyQuery" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="EasyQuery.aspx.vb" Inherits="EqDemo.EasyQuery" %>
<asp:Content ID="StylesContent" ContentPlaceHolderID="StylesPlaceHolder" runat="server">

    <link rel="stylesheet" href="https://cdn.korzh.com/eq/7.0.0/eq.core.min.css">
    <link rel="stylesheet" href="https://cdn.korzh.com/eq/7.0.0/eq.view.min.css">

    <style>
        .eqv-dropdown-content {
            min-width: 169px;
        }
        .eqv-dropdown-content a {
            font-size: 14px;
        }
    </style>
</asp:Content>

<asp:Content ID="ScriptsContent" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
  
    <!-- ChartJS script -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.1/Chart.bundle.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js" type="text/javascript"></script>

    <!-- EasyQuery script -->
    <!--<script src="https://cdn.korzh.com/eq/7.0.0/eq.community.min.js"></script>-->
    <script src="https://cdn.korzh.com/eq/7.0.0/eq.enterprise.min.js"></script>


    <!-- EasyQuery Advanced Search view initialization -->
    <script>
        window.addEventListener('load', function () {
            //Options for AdvancedSearchViewJQuery
            var viewOptions = {

                //Load model on start
                loadModelOnStart: true,
                //Load query on start
                loadQueryOnStart: false,
     
                enableExport: true,
                // Controller endpoint
                endpoint: "/api/easyquery",
                //Handlers
                handlers: {
                    //Error handler
                    onError: function (context, error) {
                        console.error(error.action + " error:\n" + error.text);
                    }
                },
                //Widgets options
                widgets: {
                    //EntitiesPanel options
                    entitiesPanel: {
                        showCheckboxes: true
                    },
                    //ColumnsPanel options
                    columnsPanel: {
                        allowAggrColumns: true,
                        allowCustomExpressions: true,
                        attrElementFormat: "{entity} {attr}",
                        titleElementFormat: "{attr}",
                        showColumnCaptions: true,
                        adjustEntitiesMenuHeight: false,
                        customExpressionText: 2,
                        showPoweredBy: false,
                        menuOptions: {
                            showSearchBoxAfter: 30,
                            activateOnMouseOver: true
                        }
                    },
                    //QueryPanel options
                    queryPanel: {
                        showPoweredBy: false,
                        alwaysShowButtonsInPredicates: false,
                        allowParameterization: true,
                        allowInJoinConditions: true,
                        autoEditNewCondition: true,
                        buttons: {
                            condition: ["menu"],
                            predicate: ["addCondition", "addPredicate", "enable", "delete"]
                        },
                        menuOptions: {
                            showSearchBoxAfter: 20,
                            activateOnMouseOver: true
                        }
                    },
                    resultGrid: {
                        autoHeight: true,
                        paging: {
                            enabled: true,
                            pageSize: 30
                        }
                    }
                },
                result: {
                    //Show EasyChart
                    showChart: true
                }
            }
            var view = new easyquery.ui.AdvancedSearchView();
            view.getContext().useEnterprise('<% Response.Write(Korzh.EasyQuery.AspNet.JSLicense.Key) %>')
            view.init(viewOptions);
            
            document['AdvancedSearchView'] = view;
        });
    </script>
</asp:Content>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="eqjs-process-bar" id="ProcessBar"></div>
    <div id="eqv-content">
        <div class="eqv-header-panel">
            <div class="eqv-entities-block">
                <hr class="eqv-entities-hr eqv-hr" />
                <div class="eqv-entities-title">Entities</div>
                <div class="eqv-entities-panel-container">
                    <!-- EntitiesPanel widget placeholder -->
                    <div id="EntitiesPanel" onselectstart="return false"></div>
                </div>
            </div>

            <div class="eqv-central-block">
                <div class="eqv-columns-block">
                    <hr class="eqv-columns-hr eqv-hr" />
                    <div class="eqv-columns-title">Columns</div>
                    <div class="eqv-columns-panel-container">
                        <!-- ColumnsPanel widget placeholder -->
                        <div id="ColumnsPanel"></div>
                    </div>
                </div>
                <div class="eqv-conditions-block">
                    <hr class="eqv-conditions-hr eqv-hr" />
                    <div class="eqv-conditions-title">Conditions</div>
                    <div class="eqv-query-panel-container">
                        <!-- QueryPanel widget placeholder -->
                        <div id="QueryPanel"></div>
                    </div>
                </div>
            </div>

            <div class="eqv-menu-block">
                <hr class="eqv-menu-hr eqv-hr" />
                <div class="eqv-menu-title">Query Menu</div>
                <div class="eqv-menu-content">
                    <div id="QueryNameLabel"></div>

                    <a id="ClearQueryButton" class="eqv-button">Clear</a>

                    <div class="eqv-dropdown-container">
                        <a id="LoadQueryButton" class="eqv-button eqv-drop-button">Load <span style="float: right">▼</span></a>
                        <div class="eqv-dropdown-content">
                        </div>
                    </div>


                    <div class="eqv-dropdown-container">
                        <a id="StorageDropButton" class="eqv-button eqv-drop-button">Storage <span style="float: right">▼</span></a>
                        <div class="eqv-dropdown-content">
                            <a id="NewQueryButton" href="#">New query</a>
                            <a id="SaveQueryButton" href="#">Save query</a>
                            <a id="CopyQueryButton" href="#">Save query as...</a>
                            <a id="RemoveQueryButton" href="#">Remove query</a>
                        </div>
                    </div>

                    <a id="FetchDataButton" class="eqv-button eqv-button-execute">Fetch data</a>

                    <div id="ChangeLocale" class="eqv-select"></div>

                    <div><p></p></div>

                </div>
            </div>
        </div>
        <div class="eqv-bottom-panel">
            <div class="eqv-sql-panel">
                <hr class="eqv-sql-panel-hr eqv-hr" />
                <div class="eqv-sql-panel-title">SQL</div>
                <div id="StatementPanel" class="eqv-sql-block">
                    <div class="sql-panel-result"></div>
                </div>
            </div>
            <div class="eqv-result-panel">
                <hr class="eqv-result-panel-hr eqv-hr" />
                <div class="eqv-result-panel-title">
                    Result
                    <span id="ResultCount" style="display:none; margin-left:20px; font-size:small"></span>
                    <span class="eqv-export-buttons">
                        <a class="eqjs-export" href="javascript:void(0)" data-format="excel-html">Export to Excel</a>
                        <a class="eqjs-export" href="javascript:void(0)" data-format="csv">Export to CSV</a>
                    </span>
                </div>
                <div id="ResultPanel" class="eqv-result-panel-content">
                </div>
            </div>
        </div>
    </div>
</asp:Content>