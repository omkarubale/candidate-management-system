import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faPlusSquare } from '@fortawesome/free-solid-svg-icons';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { ManagerJobService } from 'src/app/shared/api/manager/managerJob.service';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { CreateJobOpeningDto, GetJobOpeningCompanyDto } from 'src/app/shared/models/JobOpeningModels';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-company-manager-positions',
  templateUrl: './company-manager-positions.component.html',
  styleUrls: ['./company-manager-positions.component.css']
})
export class CompanyManagerPositionsComponent implements OnInit {

  constructor(
    private navbarService: NavbarService,
    private managerJobService: ManagerJobService,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef,
    private modalService: NgbModal,
    private router: Router
  ) {}

  //icons
  addIcon = faPlusSquare;

  positions: GetJobOpeningCompanyDto[];
  addPositionForm: CreateJobOpeningDto;
  addPositionModal: NgbModalRef;

  ngOnInit(): void {
    this.navbarService.setCurrentTab(NavbarTabs.Positions);

    this.addPositionForm = new CreateJobOpeningDto();

    this.fetchPositions();
  }

  fetchPositions() {
    this.managerJobService
    .getJobOpenings()
    .subscribe((data) => {
      this.positions = data;
    });
  }

  openAddPositionModal(content) {
    this.addPositionModal = this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' });
  }

  addJobOpening(form: CreateJobOpeningDto) {
    this.managerJobService.createJobOpening(form).subscribe(
      (result) => {
        this.toastr.success(
          'The Job Opening was created successfully.',
          'Job Opening Created!'
        );

        this.fetchPositions();

        this.cd.detectChanges();
        this.addPositionModal.close();
      },
      (error) => {
        console.error(error);
        this.toastr.error(
          error.ExceptionMessage,
          'There was an error creating this job opening!'
        );
      }
    );
  }
}
