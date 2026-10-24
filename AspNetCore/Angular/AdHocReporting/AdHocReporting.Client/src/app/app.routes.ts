import {RouterModule, Routes} from '@angular/router';
import {NgModule} from "@angular/core";
import {ReportViewComponent} from "./components/report-view/report-view.component";

export const routes: Routes = [
  {path: '', component: ReportViewComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
