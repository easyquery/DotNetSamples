import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {AppbarComponent} from "./components/appbar/appbar.component";
import {FooterComponent} from "./components/footer/footer.component";

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [RouterOutlet, AppbarComponent, FooterComponent,],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'client';
}
