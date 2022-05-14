import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { HeaderComponent } from './core/components/header/header.component';
import { FooterComponent } from './core/components/footer/footer.component';
import { PageNotFoundComponent } from './core/components/page-not-found/page-not-found.component';
import { SearchDialogComponent } from './core/components/search-dialog/search-dialog.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GraphQLModule } from './graphql.module';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    PageNotFoundComponent,
    SearchDialogComponent
  ],
  imports: [
    BrowserModule, HttpClientModule, AppRoutingModule, BrowserAnimationsModule, GraphQLModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
