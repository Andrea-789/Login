import { Component, OnInit } from '@angular/core';
import { ApiMsg } from 'src/app/model/ApiMsg';
import { UserId } from 'src/app/model/UserId';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-sendemailpwd',
  templateUrl: './sendemailpwd.component.html',
  styleUrls: ['./sendemailpwd.component.css']
})
export class SendemailpwdComponent implements OnInit {

  errMsg: string  = "";
  showErrMsg: boolean = false;
  confirm: string = "";
  
  apiMsg: ApiMsg = {
    message: ""
  }

  userId: UserId = {
    id: "",
    email: ""
  }

  constructor(private userService: UsersService) { }

  ngOnInit(): void {
  }

  sendEmail = () => {
     
    this.errMsg = "";
    this.showErrMsg = false;

    if (this.userId.email != "") {
      let regex = new RegExp("([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\"\(\[\]!#-[^-~ \t]|(\\[\t -~]))+\")@([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\[[\t -Z^-~]*])");

      if (!regex.test(this.userId.email)){
        this.errMsg = "Insert a valid Email";
        this.showErrMsg = true;
        return;
      }
    }

    this.userService.SendEmailPassword(this.userId).subscribe({
      next: (response) => {
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
