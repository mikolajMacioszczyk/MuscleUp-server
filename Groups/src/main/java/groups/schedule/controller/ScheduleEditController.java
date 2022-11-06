package groups.schedule.controller;

import groups.common.abstracts.AbstractEditController;
import groups.schedule.controller.form.ScheduleCellForm;
import groups.schedule.service.ScheduleEditService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("schedule")
public class ScheduleEditController extends AbstractEditController {

    private final ScheduleEditService scheduleEditService;
    private final ScheduleValidator scheduleValidator;


    @Autowired
    private ScheduleEditController(ScheduleEditService scheduleEditService, ScheduleValidator scheduleValidator) {

        Assert.notNull(scheduleEditService, "scheduleEditService must not be null");
        Assert.notNull(scheduleValidator, "scheduleValidator must not be null");

        this.scheduleEditService = scheduleEditService;
        this.scheduleValidator = scheduleValidator;
    }


    @PostMapping("/save")
    protected ResponseEntity<?> saveScheduleCell(@RequestBody ScheduleCellForm scheduleCellForm) {

        scheduleValidator.validateBeforeSave(scheduleCellForm, errors);

        return hasErrors() ? errors() : response(OK, scheduleEditService.save(scheduleCellForm));
    }

    @PutMapping("/single-update/{id}")
    protected ResponseEntity<?> singleUpdateScheduleCell(@PathVariable UUID id, @RequestBody ScheduleCellForm scheduleCellForm) {

        scheduleValidator.validateBeforeSingleUpdate(id, scheduleCellForm, errors);

        return hasErrors() ? errors() : response(OK, scheduleEditService.singleUpdate(id, scheduleCellForm));
    }

    @PutMapping("/cascade-update/{id}")
    protected ResponseEntity<?> cascadeUpdateScheduleCell(@PathVariable UUID id, @RequestBody ScheduleCellForm scheduleCellForm) {

        scheduleValidator.validateBeforeCascadeUpdate(id, scheduleCellForm, errors);

        return hasErrors() ? errors() : response(OK, scheduleEditService.cascadeUpdate(id, scheduleCellForm));
    }

    @DeleteMapping("/delete/{id}")
    protected ResponseEntity<?> deleteScheduleCell(@PathVariable UUID id) {

        scheduleValidator.validateBeforeDelete(id, errors);

        if (hasErrors()) return errors();

        scheduleEditService.delete(id);

        return response(OK);
    }
}

