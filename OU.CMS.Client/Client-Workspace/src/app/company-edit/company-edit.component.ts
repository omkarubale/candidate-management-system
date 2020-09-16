import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';

import { CompanyService } from '../shared/api/company.service';
import Company from '../shared/models/Company';

@Component({
  selector: 'app-company-edit',
  templateUrl: './company-edit.component.html',
  styleUrls: ['./company-edit.component.css'],
})
export class CompanyEditComponent implements OnInit, OnDestroy {
  company: Company = new Company();

  sub: Subscription;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private companyService: CompanyService
  ) {}

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.companyService.get(id).subscribe((company: any) => {
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

  save(form: any) {
    this.companyService.save(form).subscribe(
      result => {
        this.gotoList();
      },
      error => console.error(error)
    );
  }

  remove(id: number) {
    this.companyService.remove(id).subscribe(
      result => {
        this.gotoList();
      },
      error => console.error(error)
    );
  }
}
