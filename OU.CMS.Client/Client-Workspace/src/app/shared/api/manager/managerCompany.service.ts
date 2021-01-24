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
export class ManagerCompanyService {
  public API = 'https://localhost:44305/api';
  public COMPANIES_API = `${this.API}/managerCompany`;

  constructor(private http: HttpClient) {}

  getCompany(companyId: string): Observable<GetCompanyDto> {
    return this.http.get<GetCompanyDto>(
      `${this.COMPANIES_API}/GetCompany?companyId=${companyId}`
    );
  }

  editCompany(company: EditCompanyDto): Observable<GetCompanyDto> {
    return this.http.post<GetCompanyDto>(
      `${this.COMPANIES_API}/EditCompany`,
      company
    );
  }

  deleteCompany(companyId: string) {
    return this.http.delete(
      `${this.COMPANIES_API}/DeleteCompany?companyId=${companyId}`
    );
  }

  getCompanyManagement(companyId: string): Observable<GetCompanyManagementDto> {
    return this.http.get<GetCompanyManagementDto>(
      `${this.COMPANIES_API}/GetCompanyManagement?companyId=${companyId}`
    );
  }

  deleteCompanyManagement(companyManagement: DeleteCompanyManagementDto) {
    return this.http.post(
      `${this.COMPANIES_API}/DeleteCompanyManagement`,
      companyManagement
    );
  }

  createCompanyManagementInvite(
    companyManagementInvite: CreateCompanyManagementInviteDto
  ) {
    return this.http.put(
      `${this.COMPANIES_API}/CreateCompanyManagementInvite`,
      companyManagementInvite
    );
  }

  acceptCompanyManagementInvite(
    companyManagementInvite: AcceptCompanyManagementInviteDto
  ) {
    return this.http.put(
      `${this.COMPANIES_API}/AcceptCompanyManagementInvite`,
      companyManagementInvite
    );
  }

  revokeCompanyManagementInvite(
    companyManagementInvite: RevokeCompanyManagementInviteDto
  ) {
    return this.http.post(
      `${this.COMPANIES_API}/RevokeCompanyManagementInvite`,
      companyManagementInvite
    );
  }
}
