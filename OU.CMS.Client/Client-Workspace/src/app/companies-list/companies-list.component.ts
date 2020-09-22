import { Component, OnInit } from '@angular/core';
import { CompanyService } from '../shared/api/company.service';
import  { GetCompanyDto } from '../shared/models/CompanyModels';

@Component({
  selector: 'app-companies-list',
  templateUrl: './companies-list.component.html',
  styleUrls: ['./companies-list.component.css']
})
export class CompaniesListComponent implements OnInit {
  companiesList :Array<GetCompanyDto>;

  constructor(private companyService: CompanyService) { }

  ngOnInit(): void {
    this.companyService.getAllCompanies().subscribe(data => {
      this.companiesList = data;
    });
  }

}
