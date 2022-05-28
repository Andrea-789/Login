import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivationComponent } from './pages/activation/activation.component';
import { ChangepwdComponent } from './pages/changepwd/changepwd.component';
import { ErrorComponent } from './pages/error/error.component';
import { SendemailpwdComponent } from './pages/sendemailpwd/sendemailpwd.component';

import { LoginComponent } from './pages/login/login.component';
import { MainComponent } from './pages/main/main.component';
import { UserComponent } from './pages/user/user.component';

const routes: Routes = [
  {path:"", component:LoginComponent},
  {path:"login", component:LoginComponent},
  {path:"main/:user", component:MainComponent},
  {path:"user/:type", component:UserComponent},
  {path:"forgotpwd", component: SendemailpwdComponent},
  {path: "changepwd", component: ChangepwdComponent},
  {path: "activation/:id", component: ActivationComponent},
  {path:"**", component:ErrorComponent}                     //pagina di errore, deve essere sempre l'ultima
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
