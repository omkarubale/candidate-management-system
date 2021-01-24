import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import {
  EditCompanyDto,
  GetCompanyDto,
  DeleteCompanyManagementDto,
  CreateCompanyManagementInviteDto,
  AcceptCompanyManagementInviteDto,
  RevokeCompanyManagementInviteDto,
  GetCompanyManagementDto,
} from '../../models/CompanyModels';

@Injectable()
export class CandidateCompanyService {
  public API = 'https://localhost:44305/api';
  public COMPANIES_API = `${this.API}/candidateCompany`;

  constructor(private http: HttpClient) {}

  getAllCompanies(): Observable<Array<GetCompanyDto>> {
    var url = `${this.COMPANIES_API}/GetAllCompanies`;
    console.log('getAllCompanies HIT: ' + url);
    var result = this.http.get<Array<GetCompanyDto>>(url);
    console.log(result);
    return result;
  }
}
