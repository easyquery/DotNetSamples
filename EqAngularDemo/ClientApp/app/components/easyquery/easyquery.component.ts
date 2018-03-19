import { Component, ElementRef, AfterViewInit, OnInit, OnDestroy } from '@angular/core';

declare var $: any;
declare var EQ: any;

@Component({
    selector: 'easyquery',
    templateUrl: './easyquery.component.html'
})

export class EasyQueryComponent implements OnInit, AfterViewInit, OnDestroy{

    private QUERY_KEY = 'easyquerycomponent-query';

    constructor() {
      
    }
   

    ngOnInit() {
       
        //Add EasyQuery scripts dynamically

        //This script contains the settings for EasyQuery widgets
        var s = document.createElement("script");
        s.id = "settings";
        s.type = "text/javascript";
        s.src = "/js/eq.settings.js";
        $("body").prepend(s);

        //Load main EasyQuery JS scripts 
        //(optionally, you can load them from CDN)
        s = document.createElement("script");
        s.id = "eq-all";
        s.type = "text/javascript";
        //s.src = "//cdn.korzh.com/eq/4.3/eq.all.min.js";
        s.src = "/js/eq.all.min.js";
        $("body").append(s);

        s = document.createElement("script");
        s.id = "eq-view";
        s.type = "text/javascript";
        //s.src = "//cdn.korzh.com/eq/4.3/eq.view.basic.min.js";
        s.src = "/js/eq.view.basic.min.js";
        $("body").append(s);

        s = document.createElement("script");
        s.id = "easychart";
        s.type = "text/javascript";
        //s.src = "//cdn.korzh.com/eq/4.3/easychart.min.js";
        s.src = "/js/easychart.min.js";
        $("body").append(s);

        s = document.createElement("script");
        s.id = "google-chart";
        s.type = "text/javascript";
        //s.src = "//cdn.korzh.com/eq/4.3/easychart.google.min.js";
        s.src = "/js/easychart.google.min.js";
        $("body").append(s);
        
    }  

    ngAfterViewInit() {

        this.loadQueryFromLocalStorage();
    }

    ngOnDestroy() {

       this.saveQueryToLocalStorage();

        //Remove all EasyQuery scripts
        $("#settings").remove();
        $("#eq-all").remove();
        $("#eq-view").remove();
        $("#easychart").remove();
        $("#google-chart").remove();
    }

    saveQueryToLocalStorage() {
        let query = EQ.client.getQuery();
        let queryJson = query.toJSON();
  
        localStorage.setItem(this.QUERY_KEY, queryJson);
    }

    loadQueryFromLocalStorage() {
        let queryJson = localStorage.getItem(this.QUERY_KEY);
        if (queryJson) {
            EQ.client.onInit = function () {
                let query = EQ.client.getQuery();
                query.setObject(queryJson);
            };
        }
    };
    
}
