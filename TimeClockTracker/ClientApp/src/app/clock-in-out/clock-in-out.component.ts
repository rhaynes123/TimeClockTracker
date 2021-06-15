import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { isNull } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-clock-in-out',
  templateUrl: './clock-in-out.component.html'
})

export class ClockInOutComponent {
  postId;
  employeeId;
  employeeLastPunch;
  employeeUserName;
  public clockedIn: string = 'Out';
  public timepunches: Punches[];
  private _baseUrl: string;
  private currentTime: Date = new Date();
  

  constructor(private http: HttpClient
    , @Inject('BASE_URL') baseUrl: string
    , private router: Router  ) {
    this._baseUrl = baseUrl;
  }
  ngOnInit() {
    this.GetCurrentUser().subscribe(result => {
      this.employeeId = result.userId;
      this.employeeLastPunch = result.lastPunch;
      this.employeeUserName = result.userName;
    }, error => console.error(error));

    this.http.get<Punches[]>(this._baseUrl + 'TimePunches').subscribe(result => {
      this.timepunches = result;
    }, error => console.error(error));
   
  }

  public clockInOut() {
    let data = { userId: this.employeeId, userName: this.employeeUserName, lastPunch: this.currentTime.toJSON() };
    this.http.post<any>(this._baseUrl + 'TimePunches', data).subscribe(data => {
      this.postId = data.id
        , this.employeeLastPunch = data.lastPunch
        , this.clockedIn = data.clockOut === null ? 'In' : 'Out'
        , this.getTimePunches().subscribe(result => {
          this.timepunches = result;
        })
        , alert('Punch succesfully Record For Employee Id: ' + data.userId + ' ' + data.userName + ' at ' + data.lastPunch);
    })
  }

  private GetCurrentUser() {
    return this.http.get<any>(this._baseUrl + 'Users');
  }
  adjustTimePunch(id: number) {
    this.router.navigate(["/adjust-clock-in-out/" + id]);
  }

  getTimePunches() {
    return this.http.get<Punches[]>(this._baseUrl + 'TimePunches')
  }
}