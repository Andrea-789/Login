import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiMsg } from 'src/app/model/ApiMsg';
import { IChangePwd } from 'src/app/model/ChangePwd';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-changepwd',
  templateUrl: './changepwd.component.html',
  styleUrls: ['./changepwd.component.css']
})
export class ChangepwdComponent implements OnInit {

  oldPwd: string = "";
  confPwd: string = "";
  errMsg: string = "";
  showErrMsg: boolean = false;
  confirm: string = "";
  userId: string = "";

  changePwd: IChangePwd = {
    id: "",
    oldPwd: "",
    newPwd: ""
  }

  apiMsg: ApiMsg = {
    message: ""
  }
  constructor(private router: Router, private userService: UsersService, private route: ActivatedRoute) { }

  ngOnInit(): void {

    this.route.queryParams.subscribe(
      data => this.userId = data["id"]
    );

    if (this.userId != null)
      sessionStorage.setItem("userId", this.userId);
  }

  save = () => {

    this.errMsg = "";
    this.showErrMsg = false;

    if (this.changePwd.newPwd != this.confPwd) {
      this.errMsg = "Insert two equal Password";
      this.showErrMsg = true;
      return;
    }

    this.changePwd.id = "" + sessionStorage.getItem("userId");

    this.userService.changePwd(this.changePwd).subscribe({
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
