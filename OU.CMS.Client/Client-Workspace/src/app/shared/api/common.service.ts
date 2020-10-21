import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class CommonService {

  public API = 'https://localhost:44305/api';

  constructor(
    private http: HttpClient
  ) { }
}
