import { FunctionalComponent, h } from '@stencil/core';

export const AdvacnedSearchHtml: FunctionalComponent = () => (
    <div>
        <div class="eqjs-process-bar" id="ProcessBar"></div>
            <div id="main">
            <div class="eqv-header">
                    <div class="eqv-title">EasyQuery</div>
                    <div class="eqv-sub-title">Friendly ad-hoc query builder for your web-site</div>
                </div>
                <div id="eqv-content">
                    <div class="eqv-header-panel">
                        <div class="eqv-entities-block">
                            <hr class="eqv-entities-hr eqv-hr" />
                            <div class="eqv-entities-title">Entities</div>
                            <div class="eqv-entities-panel-container">
                            <div id="EntitiesPanel"></div>
                            </div>
                        </div>

                        <div class="eqv-central-block">
                            <div class="eqv-columns-block">
                                <hr class="eqv-columns-hr eqv-hr" />
                                <div class="eqv-columns-title">Columns</div>
                                <div class="eqv-columns-panel-container">

                                <div id="ColumnsPanel"></div>
                                </div>
                            </div>
                            <div class="eqv-conditions-block">
                                <hr class="eqv-conditions-hr eqv-hr" />
                                <div class="eqv-conditions-title">Conditions</div>
                                <div class="eqv-query-panel-container">
  
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
                                <a id="LoadQueryButton" href="javascript::void(0)" class="eqv-button eqv-drop-button">Load <span style={{ float: "right" }}>▼</span></a>
                                <div class="eqv-dropdown-content">
                                </div>
                            </div>

                            <div class="eqv-dropdown-container">
                                <a id="StorageDropButton" class="eqv-button eqv-drop-button">Storage <span style={{ float: "right" }}>▼</span></a>
                                <div class="eqv-dropdown-content">
                                    <a id="NewQueryButton" href="javascript::void(0)">New query</a>
                                    <a id="SaveQueryButton" href="javascript::void(0)">Save query</a>
                                    <a id="CopyQueryButton" href="javascript::void(0)">Save query as...</a>
                                    <a id="RemoveQueryButton" href="javascript::void(0)">Remove query</a>
                                </div>
                            </div>


                            <a id="ExecuteQueryButton" href="javascript::void(0)" class="eqv-button eqv-button-execute">Execute</a>
                        </div>
                    </div>
                    </div>
                     <div class="eqv-bottom-panel">
                        <div class="eqv-result-panel" style={{ width:'100%' }}>
                            <hr class="eqv-result-panel-hr eqv-hr" />
                            <div class="eqv-result-panel-title">
                                Result
                                <span id="ResultCount" style={{display:'none', marginLeft:'20px', fontSize:'small'}}></span>
                                <span class="eqv-export-buttons">
                                    <a class="eqjs-export" href="javascript:void(0)" data-format="excel-html">Export to Excel</a>
                                    <a class="eqjs-export" href="javascript:void(0)" data-format="csv">Export to CSV</a>
                                </span>
                            </div>
                            <div id="ResultPanel" class="eqv-result-panel-content">
                            </div>
                            <div class="eqv-result-panel-content-padding">
                            </div>
                        </div>

                    </div>
                </div>
                <div id="eqv-footer">
                    <div class="eqv-copyright">
                        (c) Copyright 2006-{new Date().getFullYear()} <a href="https://korzh.com/" target="_blank">Korzh.com</a>
                    </div>
                    <div class="power-by">
                        Powered by <a href="https://korzh.com/easyquery" target="_blank">EasyQuery</a>
                        |
                        <a href="http://demo.easyquerybuilder.com" target="_blank">Live demos</a>

                    </div>
                </div>
            </div>
        </div>);

export default AdvacnedSearchHtml;