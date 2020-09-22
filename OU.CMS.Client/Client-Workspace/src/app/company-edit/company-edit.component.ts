import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';

import { CompanyService } from '../shared/api/company.service';
import  { SaveCompanyDto } from '../shared/models/CompanyModels';
import { stringify } from '@angular/compiler/src/util';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-company-edit',
  templateUrl: './company-edit.component.html',
  styleUrls: ['./company-edit.component.css'],
})
export class CompanyEditComponent implements OnInit, OnDestroy {
  company: SaveCompanyDto = new SaveCompanyDto();

  sub: Subscription;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private companyService: CompanyService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.resetForm();
    this.sub = this.route.params.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.companyService.getCompany(id).subscribe((company: any) => {
          if (company) {
            this.company = company;
          } else {
            console.log(
              `Company with id '${id}' not found, returning to list`
            );
            this.gotoList();
          }
        });
      }
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  gotoList() {
    this.router.navigate(['/company-list']);
  }

  saveCompany(form: any) {
    this.companyService.saveCompany(form.value).subscribe(
      result => {
        this.toastr.success("Company was saved successfully!", "Save Successful")
        this.gotoList();
      },
      error => console.error(error)
    );
  }

  deleteCompany(companyId: string) {
    this.companyService.deleteCompany(companyId).subscribe(
      result => {
        this.gotoList();
      },
      error => console.error(error)
    );
  }

  resetForm(form?: NgForm) {
    if(form != null) {
      form.resetForm();
    }
  }
}
