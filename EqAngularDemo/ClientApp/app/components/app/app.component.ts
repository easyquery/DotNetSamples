import { Component } from '@angular/core';
import { AfterViewInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Observable } from 'rxjs/Rx';

declare var EQ: any;

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements AfterViewInit {

    ngAfterViewInit() {

        //Delete listcache when refresh page
        if (EQ && EQ.listcache && EQ.listcache.localstorage) {
            EQ.listcache.localstorage.clearCache();

            //Delete this part of code, if you don't want to clear listcache every two hours
            Observable.interval(1000 * 60 * 60 * 2).subscribe(() => {
                EQ.listcache.localstorage.clearCache();
            });
        } 
        
    }

}
