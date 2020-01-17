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
                                <div class="eqv-menu-title">Menu</div>
                                <div class="eqv-menu-content">
                                    <div id="ClearQueryButton" class="eqv-button eqv-clear-button">Clear query</div>
                                    <div id="ExecuteQueryButton" class="eqv-button eqv-execute-button">Execute</div>

                                    <div><p></p></div>
                                </div>
                            </div>
                        </div>
                        <div class="eqv-bottom-panel">
                            <div class="eqv-result-panel" style="width: 100%">
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
                                <div class="eqv-result-panel-content-padding">
                                </div>
                            </div>

                        </div>
                    </div>
                    <div id="eqv-footer">
                        <div class="eqv-copyright">
                            (c) Copyright 2006-2019 <a href="https://korzh.com/" target="_blank">Korzh.com</a>
                        </div>
                        <div class="power-by">
                            Powered by <a href="https://korzh.com/easyquery" target="_blank">EasyQuery</a>
                            |
                            <a href="http://demo.easyquerybuilder.com" target="_blank">Live demos</a>

                        </div>
                    </div>
                </div>
            </v-layout>
        </v-slide-y-transition>
    </v-container>
</template>


<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import { AdvancedSearchView, EqViewOptions } from '@easyquery/ui';
    import '@easyquery/enterprise'

    @Component({})
    export default class EasyQueryView extends Vue {

        private view = new AdvancedSearchView();
        private QUERY_KEY = 'easyqueryview-query';


        private mounted() {
            const options: EqViewOptions = {
                enableExport: true,
                loadModelOnStart: true,
                loadQueryOnStart: false,
                defaultQueryId: 'test-query',
                defaultModelId: 'NWindSQL',

                //Middlewares endpoint
                endpoint: '/api/easyquery',

                handlers: {
                    onError: (error) => {
                        // console.error(error.action + ' error:\n' + error.text);
                    }
                },
                widgets: {
                    entitiesPanel: {
                        showCheckboxes: true,
                    },
                    columnsPanel: {
                        allowAggrColumns: true,
                        allowCustomExpressions: true,
                        attrElementFormat: '{entity} {attr}',
                        titleElementFormat: '{attr}',
                        showColumnCaptions: true,
                        adjustEntitiesMenuHeight: false,
                        customExpressionText: 2,
                        showPoweredBy: false,
                        menuOptions: {
                            showSearchBoxAfter: 30,
                            activateOnMouseOver: true,
                        },
                    },
                    queryPanel: {
                        showPoweredBy: false,
                        alwaysShowButtonsInPredicates: false,
                        allowParameterization: true,
                        allowInJoinConditions: true,
                        autoEditNewCondition: true,
                        buttons: {
                            condition: ['menu'],
                            predicate: ['addCondition', 'addPredicate', 'enable', 'delete'],
                        },
                        menuOptions: {
                            showSearchBoxAfter: 20,
                            activateOnMouseOver: true,
                        },
                    },
                },
                result: {
                    showChart: true,
                },
            };

            this.view.getContext().useEnterprise(() => {
                this.view.init(options);
            });

            this.view.getContext().addEventListener('ready', () => {
                  // here we need to add query autosave
                const query = this.view.getContext().getQuery();

                query.addChangedCallback(() => {
                    const queryJson = query.toJSON();
                    localStorage.setItem(this.QUERY_KEY, queryJson);
                    // console.log('Query saved', query);
                });

                // add load query from local storage
                this.loadQueryFromLocalStorage();
            });
        }

        private loadQueryFromLocalStorage() {
            const queryJson = localStorage.getItem(this.QUERY_KEY);
            if (queryJson) {
                const query = this.view.getContext().getQuery();
                query.loadFromDataOrJson(queryJson);
                query.fireChangedEvent();

                setTimeout(() => this.view.executeQuery(), 100);
            }
        }


    }
</script>