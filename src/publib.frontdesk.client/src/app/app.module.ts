import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FrontDeskComponent } from './features/front-desk/front-desk.component';
import { LayoutComponent } from './layout/layout.component';

@NgModule({
    declarations: [
        AppComponent,
        FrontDeskComponent,
        LayoutComponent
    ],
    imports: [
        BrowserModule, HttpClientModule,
        AppRoutingModule,
        FormsModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
