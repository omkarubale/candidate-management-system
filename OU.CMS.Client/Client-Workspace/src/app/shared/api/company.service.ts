import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import Company from '../models/Company';

@Injectable()
export class CompanyService {
  public API = 'https://localhost:44305/api';
  public COMPANIES_API = `${this.API}/company`;
  formData : Company;

  constructor(
    private http: HttpClient
  ) { }

  getAllCompanies(): Observable<Array<Company>> {
    var url = `${this.COMPANIES_API}/GetAllCompanies`;
    console.log("getAllCompanies HIT: " + url);
    var result = this.http.get<Array<Company>>(url);
    console.log(result);
    return result;
  }

  getCompany(companyId: string) {
    return this.http.get(`${this.COMPANIES_API}/GetCompany?companyId=${companyId}`);
  }

  saveCompany(company: Company): Observable<Company> {
    let result: Observable<Company>;
    if (company.id) {
      result = this.http.put<Company>(`${this.COMPANIES_API}/CreateCompany`, company);
    } else {
      result = this.http.post<Company>(`${this.COMPANIES_API}/UpdateCompany`, company);
    }
    return result;
  }

  deleteCompany(companyId: string) {
    return this.http.delete(`${this.COMPANIES_API}/DeleteCompany?companyId=${companyId}`);
  }
}
