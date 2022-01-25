import React  from 'react';

const AdvacnedSearchHtml = () => (
    <div>
        <div className="eqjs-process-bar" id="ProcessBar"></div>
            <div id="main">
            <div className="eqv-header">
                    <div className="eqv-title">EasyQuery</div>
                    <div className="eqv-sub-title">Friendly ad-hoc query builder for your web-site</div>
                </div>
                <div id="eqv-content">
                    <div className="eqv-header-panel">
                        <div className="eqv-entities-block">
                            <hr className="eqv-entities-hr eqv-hr" />
                            <div className="eqv-entities-title">Entities</div>
                            <div className="eqv-entities-panel-container">
                            <div id="EntitiesPanel"></div>
                            </div>
                        </div>

                        <div className="eqv-central-block">
                            <div className="eqv-columns-block">
                                <hr className="eqv-columns-hr eqv-hr" />
                                <div className="eqv-columns-title">Columns</div>
                                <div className="eqv-columns-panel-container">

                                <div id="ColumnsPanel"></div>
                                </div>
                            </div>
                            <div className="eqv-conditions-block">
                                <hr className="eqv-conditions-hr eqv-hr" />
                                <div className="eqv-conditions-title">Conditions</div>
                                <div className="eqv-query-panel-container">
  
                                <div id="QueryPanel"></div>
                                </div>
                            </div>
                    </div>
                    <div className="eqv-menu-block">
                        <hr className="eqv-menu-hr eqv-hr" />
                        <div className="eqv-menu-title">Query Menu</div>
                        <div className="eqv-menu-content">

                            <div id="QueryNameLabel"></div>


                            <a id="ClearQueryButton" className="eqv-button">Clear</a>


                            <div className="eqv-dropdown-container">
                                <a id="LoadQueryButton" href="javascript::void(0)" className="eqv-button eqv-drop-button">Load <span style={{ float: "right" }}>▼</span></a>
                                <div class="eqv-dropdown-content">
                                </div>
                            </div>

                            <div class="eqv-dropdown-container">
                                <a id="StorageDropButton" className="eqv-button eqv-drop-button">Storage <span style={{ float: "right" }}>▼</span></a>
                                <div class="eqv-dropdown-content">
                                    <a id="NewQueryButton" href="javascript::void(0)">New query</a>
                                    <a id="SaveQueryButton" href="javascript::void(0)">Save query</a>
                                    <a id="CopyQueryButton" href="javascript::void(0)">Save query as...</a>
                                    <a id="RemoveQueryButton" href="javascript::void(0)">Remove query</a>
                                </div>
                            </div>


                            <a id="FetchDataButton" href="javascript::void(0)" className="eqv-button eqv-button-execute">Fetch data</a>
                        </div>
                    </div>
                    </div>
                     <div className="eqv-bottom-panel">
                        <div className="eqv-result-panel" style={{ width:'100%' }}>
                            <hr className="eqv-result-panel-hr eqv-hr" />
                            <div className="eqv-result-panel-title">
                                Result
                          <span id="ResultCount" style={{ display:'none', marginLeft:20, fontSize:'small'}}></span>
                                <span className="eqv-export-buttons">
                                    <a className="eqjs-export" href="javascript:void(0)" data-format="pdf">Export to PDF</a>
                                    <a className="eqjs-export" href="javascript:void(0)" data-format="excel">Export to Excel</a>
                                    <a className="eqjs-export" href="javascript:void(0)" data-format="csv">Export to CSV</a>
                                </span>
                            </div>
                            <div id="ResultPanel" className="eqv-result-panel-content">
                            </div>
                            <div className="eqv-result-panel-content-padding">
                            </div>
                        </div>

                    </div>
                </div>
                <div id="eqv-footer">
                <div className="eqv-copyright">
                    (c) Copyright 2006-{new Date().getFullYear()} <a href="https://korzh.com/" target="_blank">Korzh.com</a> | 
                    </div>
                    <div className="power-by">
                        Powered by <a href="https://korzh.com/easyquery" target="_blank">EasyQuery</a>

                    </div>
                </div>
            </div>
        </div>);

export default AdvacnedSearchHtml;
