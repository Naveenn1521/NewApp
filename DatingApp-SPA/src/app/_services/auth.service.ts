import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = 'http://localhost:5000/api/Auth/';

constructor(private http: HttpClient) { }

login(model: any) {
  this.http.post(this.baseUrl + 'Login', model).pipe(
    map((response: any) => {
      const user = response;
      if (user) {
     localStorage.setItem('key', user.token);
      }
    })
  );
}
register(model: any) {
  return this.http.post(this.baseUrl + 'Register', model);
}
}
