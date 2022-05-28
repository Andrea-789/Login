import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiMsg } from 'src/app/model/ApiMsg';
import { UserId } from 'src/app/model/UserId';
import { UsersService } from 'src/app/services/users.service';
import { UserComponent } from '../user/user.component';

@Component({
  selector: 'app-activation',
  templateUrl: './activation.component.html',
  styleUrls: ['./activation.component.css']
})
export class ActivationComponent implements OnInit {

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

  constructor(private route: ActivatedRoute, private userService: UsersService) { }

  ngOnInit(): void {
    
    this.userId.id = this.route.snapshot.params["id"];
    //console.log("id=", this.userId.id);

    if (this.userId.id == null || this.userId.id == ""){
      this.errMsg = "No user to activate";
      this.showErrMsg = true;
      return;
    }

    this.userService.Activation(this.userId).subscribe({
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
