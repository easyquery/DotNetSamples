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
                            <div className="eqv-menu-title">Menu</div>
                            <div className="eqv-menu-content">
                                <div id="ClearQueryButton" className="eqv-button eqv-clear-button">Clear query</div>
	                        <div id="ExecuteQueryButton" className="eqv-button eqv-execute-button">Execute</div>

                                <div><p></p></div>
             
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
                                    <a className="eqjs-export" href="javascript:void(0)" data-format="excel-html">Export to Excel</a>
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
                        (c) Copyright 2006-2019 <a href="https://korzh.com/" target="_blank">Korzh.com</a>
                    </div>
                    <div className="power-by">
                        Powered by <a href="https://korzh.com/easyquery" target="_blank">EasyQuery</a>
                        |
              <a href="http://demo.easyquerybuilder.com" target="_blank">Live demos</a>

                    </div>
                </div>
            </div>
        </div>);

export default AdvacnedSearchHtml;
