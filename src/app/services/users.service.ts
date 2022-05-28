import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ApiMsg } from '../model/ApiMsg';
import { IChangePwd } from '../model/ChangePwd';
import { IUser } from '../model/User.model';
import { UserId } from '../model/UserId';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private httpClient: HttpClient) { }

  getUserByUsername = (username: string) =>
    this.httpClient.get<IUser>(environment.userServerUri + "usr/" + username);

  getUserById = (id: string) =>
    this.httpClient.get<IUser>(environment.userServerUri + "id/" + id);

  insUser = (user: IUser) =>
    this.httpClient.post<ApiMsg>(environment.userServerUri + "insert", user);

  updUser = (user: IUser) =>
    this.httpClient.put<ApiMsg>(environment.userServerUri + "update", user);

  changePwd = (changePwd: IChangePwd) =>
    this.httpClient.put<ApiMsg>(environment.userServerUri + "changepwd", changePwd);

  Activation = (userId: UserId) =>
    this.httpClient.put<ApiMsg>(environment.userServerUri + "activation", userId);

  SendActivationEmail = (email: string) => 
    this.httpClient.get<ApiMsg>(environment.userServerUri + "sendactivationemail/" + email);
  
  SendEmailPassword = (userId: UserId) => 
    this.httpClient.put<ApiMsg>(environment.userServerUri + "sendpasswordemail", userId)
}
