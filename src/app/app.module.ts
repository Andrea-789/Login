import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './pages/login/login.component';
import { ErrorComponent } from './pages/error/error.component';
import { MainComponent } from './pages/main/main.component';
import { UserComponent } from './pages/user/user.component';
import { SendemailpwdComponent } from './pages/sendemailpwd/sendemailpwd.component';
import { ChangepwdComponent } from './pages/changepwd/changepwd.component';
import { ActivationComponent } from './pages/activation/activation.component';
import { SpinnerComponent } from './pages/commons/spinner/spinner.component';
import { NetworkInterceptor } from './services/interceptors/network.interceptor';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ErrorComponent,
    MainComponent,
    UserComponent,
    SendemailpwdComponent,
    ChangepwdComponent,
    ActivationComponent,
    SpinnerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: NetworkInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
