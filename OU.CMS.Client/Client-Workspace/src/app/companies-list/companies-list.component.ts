import { Component, OnInit } from '@angular/core';
import { CompanyService } from '../shared/api/company.service';
import Company from '../shared/models/Company';

@Component({
  selector: 'app-companies-list',
  templateUrl: './companies-list.component.html',
  styleUrls: ['./companies-list.component.css']
})
export class CompaniesListComponent implements OnInit {
  companiesList :Array<Company>;

  constructor(private companyService: CompanyService) { }

  ngOnInit(): void {
    this.companyService.getAll().subscribe(data => {
      this.companiesList = data;
    });
  }

}
