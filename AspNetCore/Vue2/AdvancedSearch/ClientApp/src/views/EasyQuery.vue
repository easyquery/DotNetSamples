<template>
    <v-container fluid>
        <v-slide-y-transition mode="out-in">
            <v-layout column>
                <div class="eqjs-process-bar" id="ProcessBar"></div>
                <div id="main">
                    <div class="eqv-header">
                        <div class="eqv-title">EasyQuery</div>
                        <div class="eqv-sub-title">Friendly ad-hoc query builder for your web-site</div>
                        <div class="eqv-global-message">@ViewBag.Message</div>
                    </div>
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
                                        <a id="LoadQueryButton" href="javascript::void(0)" class="eqv-button eqv-drop-button">Load <span style="float: right">▼</span></a>
                                        <div class="eqv-dropdown-content">
                                        </div>
                                    </div>

                                    <div class="eqv-dropdown-container">
                                        <a id="StorageDropButton" class="eqv-button eqv-drop-button">Storage <span style="float: right">▼</span></a>
                                        <div class="eqv-dropdown-content">
                                            <a id="NewQueryButton" href="javascript::void(0)">New query</a>
                                            <a id="SaveQueryButton" href="javascript::void(0)">Save query</a>
                                            <a id="CopyQueryButton" href="javascript::void(0)">Save query as...</a>
                                            <a id="RemoveQueryButton" href="javascript::void(0)">Remove query</a>
                                        </div>
                                    </div>


                                    <a id="FetchDataButton" href="javascript::void(0)" class="eqv-button eqv-button-execute">Fetch data</a>
                                </div>
                            </div>
                            <div class="eqv-bottom-panel">
                                <div class="eqv-result-panel" style="width: 100%">
                                    <hr class="eqv-result-panel-hr eqv-hr" />
                                    <div class="eqv-result-panel-title">
                                        Result
                                        <span id="ResultCount" style="display:none; margin-left:20px; font-size:small"></span>
                                        <span class="eqv-export-buttons">
                                            <a class="eqjs-export" href="javascript:void(0)" data-format="pdf">Export to PDF</a>
                                            <a class="eqjs-export" href="javascript:void(0)" data-format="excel">Export to Excel</a>
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
                    </div>
                    <div id="eqv-footer">
                        <div class="eqv-copyright">
                            (c) Copyright 2006-2021 <a href="https://korzh.com/" target="_blank">Korzh.com</a>
                        </div>
                        <div class="power-by">
                            Powered by <a href="https://korzh.com/easyquery" target="_blank">EasyQuery</a>
                            |
                            <a href="https://korzh.com/demo" target="_blank">Live demos</a>

                        </div>
                    </div>
                </div>
            </v-layout>
        </v-slide-y-transition>
    </v-container>
</template>


<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import { EqContext } from '@easyquery/core';
    import { AdvancedSearchView, EqViewOptions } from '@easyquery/ui';
    import '@easyquery/enterprise'

    @Component({})
    export default class EasyQueryView extends Vue {

        private view: AdvancedSearchView;
        private context: EqContext;
        private QUERY_KEY = 'easyqueryview-query';


        private mounted() {
            const viewOptions: EqViewOptions = {
                enableExport: true,
                serverExporters: ['pdf', 'excel', 'csv'],
                loadModelOnStart: true,

                handlers: {
                    onError: (_, error) => {
                        console.error(error.sourceError);
                    }
                },
                widgets: {
                    resultGrid: {
                        paging: {
                            enabled: true,
                            pageSize: 30
                        }
                    }
                },
                result: {
                    showChart: true,
                },
            };

            this.view = new AdvancedSearchView();
            this.context = this.view.getContext();

            this.context
                .useEndpoint('/api/easyquery')
                .useEnterprise(() => {
                    this.view.init(viewOptions);
                });

            this.context.addEventListener('ready', () => {
                const query = this.context.getQuery();

                query.addChangedCallback(() => {
                    const data = JSON.stringify({
                        modified: query.isModified(),
                        query: query.toJSONData()
                    });
                    localStorage.setItem(this.QUERY_KEY, data);
                });

                //add load query from local storage
                this.loadQueryFromLocalStorage();
            });
        }

        private loadQueryFromLocalStorage() {
            const dataJson = localStorage.getItem(this.QUERY_KEY);
            if (dataJson) {
                const data = JSON.parse(dataJson);
                const query = this.context.getQuery();
                query.loadFromDataOrJson(data.query);
                if (data.modified) {
                    query.fireChangedEvent();
                }
                else {
                    this.view.getContext().refreshWidgets();
                    this.view.syncQuery();
                }

                setTimeout(() => this.view.fetchData(), 100);
            }
        };    

    }
</script>