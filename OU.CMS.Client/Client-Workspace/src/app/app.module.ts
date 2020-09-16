import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CompaniesListComponent } from './companies-list/companies-list.component';
import { CompanyService } from './shared/api/company.service';
import { CompanyEditComponent } from './company-edit/company-edit.component';
import { Routes } from '@angular/router';

const appRoutes: Routes = [
  { path: '', redirectTo: '/company-list', pathMatch: 'full' },
  {
    path: 'company-list',
    component: CompaniesListComponent
  },
  {
    path: 'company-add',
    component: CompanyEditComponent
  },
  {
    path: 'company-edit/:id',
    component: CompanyEditComponent
  }
];

@NgModule({
  declarations: [
    AppComponent,
    CompaniesListComponent,
    CompanyEditComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [CompanyService],
  bootstrap: [AppComponent]
})
export class AppModule { }
