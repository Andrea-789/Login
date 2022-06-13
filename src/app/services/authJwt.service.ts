import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Token } from '../model/Token';

@Injectable({
  providedIn: 'root'
})
export class AuthJwtService {

  constructor(private httpClient: HttpClient) { }

  authenticateService(User: string, Password: string) {

    console.log(`${environment.userServerUri}auth`);
    console.log(User, Password);

    return this.httpClient.post<Token>(
      `${environment.userServerUri}auth`, { User, Password }).pipe(
        map(
          data => {
            sessionStorage.setItem("User", User);
            sessionStorage.setItem("userId", data.id)
            sessionStorage.setItem("expired", data.expired)
            sessionStorage.setItem("AuthToken", `Bearer ${data.token}`)
            sessionStorage.setItem("enabled", data.enabled);
            return data;
          }
        )
      );
  }

  getAuthToken = (): string => {

    let AuthHeader: string = "";
    let AuthToken = sessionStorage.getItem("AuthToken");

    if (AuthToken != null)
      AuthHeader = AuthToken;

    return AuthHeader;
  }

  loggedUser = (): string | null => (sessionStorage.getItem("User")) ? sessionStorage.getItem("User") : "";

  isLogged = (): boolean => (sessionStorage.getItem("User")) ? true : false;

  clearUser = (): void => (sessionStorage.removeItem("User"));

  clearAll = (): void => (sessionStorage.clear());

}
