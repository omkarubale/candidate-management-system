import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/shared/api/account.service';
import { CompanyService } from 'src/app/shared/api/company.service';
import { CompanySimpleDto, GetCompanyDto } from 'src/app/shared/models/CompanyModels';

@Component({
  selector: 'app-company-manager-company',
  templateUrl: './company-manager-company.component.html',
  styleUrls: ['./company-manager-company.component.css'],
})
export class CompanyManagerCompanyComponent implements OnInit {
  constructor(
    private accountService: AccountService,
    private companyService: CompanyService
  ) {}

  companyDetails: GetCompanyDto;

  ngOnInit(): void {
    this.companyService.getCompany(this.accountService.userInfo.CompanyId).subscribe(data => {
      this.companyDetails = data;
    });
  }
}
