import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
@Component({
  selector: 'app-adjust-clock-in-out',
  templateUrl: './adjust-clock-in-out.component.html',
  styleUrls: ['./adjust-clock-in-out.component.css']
})
export class AdjustClockInOutComponent implements OnInit {
  adjustTimePunchForm: FormGroup;
  timePunch: Punches;
  private _baseUrl: string;
  private currentTime: Date = new Date();

  constructor(private http: HttpClient
    , @Inject('BASE_URL') baseUrl: string
    , private route: ActivatedRoute
    , private router: Router
    , private fb: FormBuilder) {
    this._baseUrl = baseUrl;
  }

  ngOnInit() {
    this.getById(this.route.snapshot.params.id).subscribe(result => {
      this.timePunch = result

      this.adjustTimePunchForm = this.fb.group({
        id: [result.id],
        userName: [result.userName],
        clockIn: [result.clockIn],
        clockOut: [result.clockOut],
        userId: [result.userId],
        lastPunch: [result.lastPunch],
      })
    })
  };

  adjustPunch(timePunch: Punches) {
    this.http.put<Punches>(this._baseUrl + 'TimePunches/AdjustPunch/' + timePunch.id, timePunch).subscribe(data => {
      this.router.navigate(["/fetch-data"]);
        alert('Punch succesfully Record For Employee Id: ' + data.userId + ' ' + data.userName + ' at ' + data.lastPunch);
    })
  };

  getById(id: number) {
    return this.http.get<Punches>(this._baseUrl + 'TimePunches/GetPunchById/'+id);
  };

  onSubmit() {
    this.adjustPunch(this.adjustTimePunchForm.value);
  }
}
