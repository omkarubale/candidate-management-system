import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import Company from '../models/Company';

@Injectable()
export class CompanyService {
  public API = 'http://localhost:8080/api';
  public COMPANIES_API = `${this.API}/comapanies`;

  constructor(
    private http: HttpClient
  ) { }

  getAll(): Observable<Array<Company>> {
    return this.http.get<Array<Company>>(this.COMPANIES_API);
  }

  get(id: string) {
    return this.http.get(`${this.COMPANIES_API}/${id}`);
  }

  save(company: Company): Observable<Company> {
    let result: Observable<Company>;
    if (company.id) {
      result = this.http.put<Company>(
        `${this.COMPANIES_API}/${company.id}`,
        company
      );
    } else {
      result = this.http.post<Company>(this.COMPANIES_API, company);
    }
    return result;
  }

  remove(id: number) {
    return this.http.delete(`${this.COMPANIES_API}/${id.toString()}`);
  }
}
