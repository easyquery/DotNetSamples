import { Component, ElementRef, AfterViewInit, OnInit} from '@angular/core';

declare var $: any;
declare var EQ: any;

@Component({
    selector: 'easyquery',
    templateUrl: './easyquery.component.html'
})

export class EasyQueryComponent implements OnInit, AfterViewInit{

    private QUERY_KEY = 'easyquerycomponent-query';

    constructor() {
      
    }
   

    ngOnInit() {
       
        //Add EasyQuery scripts dynamically
        //This script contains the settings for EasyQuery widgets
        if ($("#settings").length === 0) {
            var s = document.createElement("script");
            s.id = "settings";
            s.type = "text/javascript";
            s.src = "/js/eq.settings.js";
            $("body").prepend(s);
            console.log("settings created");
        }

        if ($("#eq-all").length === 0) {
            //Load main EasyQuery JS scripts 
            //(optionally, you can load them from CDN)
            s = document.createElement("script");
            s.id = "eq-all";
            s.type = "text/javascript";
            //s.src = "//cdn.korzh.com/eq/4.3/eq.all.min.js";
            s.src = "/js/eq.all.min.js";
            $("body").append(s);
        }

        if ($("#eq-view").length === 0) {
            s = document.createElement("script");
            s.id = "eq-view";
            s.type = "text/javascript";
            //s.src = "//cdn.korzh.com/eq/4.3/eq.view.basic.min.js";
            s.src = "/js/eq.view.basic.min.js";
            $("body").append(s);
        } else {
            EQ.view.basic.init();
        }

        if ($("#easychart").length === 0) {
            s = document.createElement("script");
            s.id = "easychart";
            s.type = "text/javascript";
            //s.src = "//cdn.korzh.com/eq/4.3/easychart.min.js";
            s.src = "/js/easychart.min.js";
            $("body").append(s);
        }

        if ($("#google-chart") === 0) {
            s = document.createElement("script");
            s.id = "google-chart";
            s.type = "text/javascript";
            //s.src = "//cdn.korzh.com/eq/4.3/easychart.google.min.js";
            s.src = "/js/easychart.google.min.js";
            $("body").append(s);
        }

        
    }  

    ngAfterViewInit() {
        this.loadQueryFromLocalStorage();

        let query = EQ.client.getQuery();
        let self = this;

        query.addChangedCallback(() => {
            let queryJson = query.toJSON();
            localStorage.setItem(self.QUERY_KEY, queryJson);
        });
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
