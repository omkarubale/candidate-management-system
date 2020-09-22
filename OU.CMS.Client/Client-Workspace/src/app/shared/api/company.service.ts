import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import  {
  SaveCompanyDto,
  GetCompanyDto,
  DeleteCompanyManagementDto,
  CreateCompanyManagementInviteDto,
  AcceptCompanyManagementInviteDto,
  RevokeCompanyManagementInviteDto
} from '../models/CompanyModels';

@Injectable()
export class CompanyService {
  public API = 'https://localhost:44305/api';
  public COMPANIES_API = `${this.API}/company`;

  constructor(
    private http: HttpClient
  ) { }

  getAllCompanies(): Observable<Array<GetCompanyDto>> {
    var url = `${this.COMPANIES_API}/GetAllCompanies`;
    console.log("getAllCompanies HIT: " + url);
    var result = this.http.get<Array<GetCompanyDto>>(url);
    console.log(result);
    return result;
  }

  getCompany(companyId: string): Observable<GetCompanyDto> {
    return this.http.get<GetCompanyDto>(`${this.COMPANIES_API}/GetCompany?companyId=${companyId}`);
  }

  saveCompany(company: SaveCompanyDto): Observable<GetCompanyDto> {
    return this.http.post<GetCompanyDto>(`${this.COMPANIES_API}/SaveCompany`, company);
  }

  deleteCompany(companyId: string) {
    return this.http.delete(`${this.COMPANIES_API}/DeleteCompany?companyId=${companyId}`);
  }

  deleteCompanyManagement(companyManagement: DeleteCompanyManagementDto) {
    return this.http.post(`${this.COMPANIES_API}/DeleteCompanyManagement`, companyManagement);
  }

  createCompanyManagementInvite(companyManagementInvite: CreateCompanyManagementInviteDto) {
    return this.http.put(`${this.COMPANIES_API}/CreateCompanyManagementInvite`, companyManagementInvite);
  }

  acceptCompanyManagementInvite(companyManagementInvite: AcceptCompanyManagementInviteDto) {
    return this.http.put(`${this.COMPANIES_API}/AcceptCompanyManagementInvite`, companyManagementInvite);
  }

  revokeCompanyManagementInvite(companyManagementInvite: RevokeCompanyManagementInviteDto) {
    return this.http.put(`${this.COMPANIES_API}/RevokeCompanyManagementInvite`, companyManagementInvite);
  }
}
