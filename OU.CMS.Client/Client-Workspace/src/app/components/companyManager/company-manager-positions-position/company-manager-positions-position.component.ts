import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { ManagerJobService } from 'src/app/shared/api/manager/managerJob.service';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { GetCandidateDto } from 'src/app/shared/models/CandidateModels';
import { GetJobOpeningDto, UpdateJobOpeningDto } from 'src/app/shared/models/JobOpeningModels';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-company-manager-positions-position',
  templateUrl: './company-manager-positions-position.component.html',
  styleUrls: ['./company-manager-positions-position.component.css'],
})
export class CompanyManagerPositionsPositionComponent implements OnInit {
  constructor(
    private navbarService: NavbarService,
    private managerJobService: ManagerJobService,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  //icons
  deleteIcon = faTrash;

  currentPositionId: string;
  positionDetails: GetJobOpeningDto = new GetJobOpeningDto;
  positionCandidates: GetCandidateDto[];

  ngOnInit(): void {
    this.navbarService.setCurrentTab(NavbarTabs.Positions);

    this.route.params.subscribe((params) => {
      this.currentPositionId = params['id'];

      if (this.currentPositionId) {
        this.managerJobService
          .getJobOpening(this.currentPositionId)
          .subscribe((data) => {
            if (data) {
              this.positionDetails = data;
            } else {
              console.log(
                `Position with id '${this.currentPositionId} was not found!'`
              );
              this.gotoPositionsIndex();
            }
          });

        this.fetchPositionCandidates();
      } else {
        this.gotoPositionsIndex();
      }
    });
  }

  // Position
  editPosition(form: UpdateJobOpeningDto) {
    this.managerJobService.updateJobOpening(form).subscribe(
      (result) => {
        this.toastr.success(
          'The job position was saved successfully.',
          'Job Opening Updated!'
        );
      },
      (error) => {
        console.error(error);
        this.toastr.error(
          error.message,
          'There was an error saving the job Opening!'
        );
      }
    );
  }

  deletePosition() {
    this.managerJobService.deleteJobOpening(this.currentPositionId).subscribe(
      () => {
        this.toastr.success(
          'The job position was delete successfully.',
          'Job Opening Deleted!'
        );
        this.gotoPositionsIndex();
      },
      (error) => {
        console.error(error);
        this.toastr.error(
          error.message,
          'There was an error deleting the job Opening!'
        );
      }
    );
  }

  gotoPositionsIndex() {
    this.router.navigate(['/companyManager-positions']);
  }

  // Position Candidates
  fetchPositionCandidates() {
    this.managerJobService
    .getCandidatesForJobOpening(this.currentPositionId)
    .subscribe((data) => {
      this.positionCandidates = data;
    });
  }
}
