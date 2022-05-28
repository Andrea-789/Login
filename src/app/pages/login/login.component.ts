import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiMsg } from 'src/app/model/ApiMsg';
import { AuthJwtService } from 'src/app/services/authJwt.service';
import { UsersService } from 'src/app/services/users.service';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  email: string = "";
  password: string = "";
  errMsg: string = "Wrong Emal or Password!";
  showErrMsg: boolean = false;
  showPwdExpired: boolean = false;
  showMsgEnabled: boolean = false;
  confirm: string = "";

  apiMsg: ApiMsg = {
    message: ""
  }
  
  constructor(private router: Router, private Auth: AuthJwtService, private user: UsersService ) { }

  ngOnInit(): void {
  }

  gestAuth = () => {

    if (!environment.production) {
      console.log(this.email);
      console.log(this.password);
    }
    
    this.Auth.authenticateService(this.email, this.password).subscribe({
      next: (response) => {
        if (!environment.production)
          console.log("authenticateService",response);                
        
        let expired = sessionStorage.getItem("expired");
        let enabled = sessionStorage.getItem("enabled");

        if (!environment.production){
          console.log("enabled = " + enabled);
          console.log("expired = " + expired);          
        }

        if (expired == null || expired == "true") {
          sessionStorage.removeItem("expired");
          sessionStorage.removeItem("enabled");       
          this.errMsg = "Password expired";
          this.showErrMsg = true;
          this.showPwdExpired = true;
          this.showMsgEnabled = false;
        }
        else if (enabled == null || enabled == "0")
        {
          sessionStorage.removeItem("enabled");
          sessionStorage.removeItem("expired");
          this.errMsg = "User not enabled";
          this.showErrMsg = true;
          this.showMsgEnabled = true;
        }
        else
        {
          sessionStorage.removeItem("enabled");
          sessionStorage.removeItem("expired");
          this.router.navigate(["main", this.email]);
        }
      },
      error: (error) => {
        if (!environment.production)
          console.log("error", error);

        this.errMsg = error.error.message;
        this.showErrMsg = true;
      }
    });
  }
  
  SendEmail = () => {
    this.user.SendActivationEmail(this.email).subscribe({
      next: (response) => {
        this.apiMsg = response;
        this.confirm = this.apiMsg.message;
        this.errMsg = "";
        this.showErrMsg = false;
        this.showMsgEnabled = false;
      },
      error: (error) => {
        this.errMsg = error.error.message;
      }
    });
  }
}
