import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiMsg } from 'src/app/model/ApiMsg';
import { IUser } from 'src/app/model/User.model';
import { UsersService } from 'src/app/services/users.service';
import { environment } from 'src/environments/environment';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  constructor(private route: ActivatedRoute, private userService: UsersService,
    private router: Router) { }

  type: string = ""  ;
  id: string = "";
  username: string = "";
  title: string = "";
  isEdit: boolean = false;
  confPassword: string = "";

  user: IUser = {
    Id: "",
    Name: "",
    Surname: "",
    Address: "",
    ZIP: "",
    City: "",
    State: "",
    Telephone: "",
    CompanyName: "",
    STN: "",
    SSN: "",
    Email: "",
    Password: "",
    Enabled: 0,
    Date: new Date(),
    Role: ""
  }

  apiMsg: ApiMsg = {
    message: ""
  }

  confirm: string = "";
  errMsg: string = "";
  showErrMsg: boolean = false;

  ngOnInit(): void {
    this.type = this.route.snapshot.params["type"];
    this.id = this.route.snapshot.params["id"];

    console.log(this.type);
    console.log(this.username);
    
    if (this.id) {
      this.title = "Edit";
      this.isEdit = true;

      this.userService.getUserById(this.id).subscribe({
        next: this.handleResponse.bind(this),
        error: this.handleError.bind(this)
      });
    } else {
      if (this.type === "account")
        this.title = "Register";
      else
        this.title = "Insert";

      this.isEdit = false;
    }

  }

  handleResponse = (response: any) => {
    this.user = response;

    if (!environment.production)
      console.log(this.user);
  }

  handleError = (error: any) => {
    this.errMsg = error;
    this.showErrMsg = true;
  }

  save = () => {

    this.errMsg = "";
    this.showErrMsg = false;

    if (!this.checkFields())
      return;

    console.log(this.user);

    if (this.isEdit) {
      this.userService.updUser(this.user).subscribe({
        next: (response) => {
          this.apiMsg = response;
          this.confirm = this.apiMsg.message;
          this.errMsg = "";
          this.showErrMsg = false;
        },
        error: (error) => {
          this.errMsg = error.error.message;
          this.showErrMsg = true;
        }
      });
    } else {
      this.userService.insUser(this.user).subscribe({
        next: (response) => {
          console.log("response", response);
          this.apiMsg = response;
          this.confirm = this.apiMsg.message;
          this.errMsg = "";
          this.showErrMsg = false;
        },
        error: (error) => {
          console.log("error", error);
          this.errMsg = error.error.message;
          this.showErrMsg = true;
        }
      });
    }
  }

  checkFields = (): boolean => {

    if (this.user.Name === "" && this.user.Surname === "" && this.user.CompanyName === "") {
      this.errMsg = "Insert Name and Surname or Company Name";
      this.showErrMsg = true;
      return false;
    }

    if (this.user.CompanyName != "" && this.user.STN === "") {
      this.errMsg = "Insert a valid STN";
      this.showErrMsg = true;
      return false;
    }

    if (this.user.Email != "") {
      let regex = new RegExp("([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\"\(\[\]!#-[^-~ \t]|(\\[\t -~]))+\")@([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\[[\t -Z^-~]*])");

      if (!regex.test(this.user.Email)) {
        this.errMsg = "Insert a valid Email";
        this.showErrMsg = true;
        return false;
      }
    }

    if (this.user.Password != this.confPassword) {
      this.errMsg = "Insert two equal Password";
      this.showErrMsg = true;
      return false;
    }

    return true;

  }
}
