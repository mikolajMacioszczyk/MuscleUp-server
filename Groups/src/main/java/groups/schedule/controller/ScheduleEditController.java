package groups.schedule.controller;

import groups.common.abstracts.AbstractEditController;
import groups.groupWorkout.controller.form.GroupWorkoutForm;
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


    @PostMapping
    protected ResponseEntity<?> saveScheduleCell(@RequestBody ScheduleCellForm scheduleCellForm) {

        scheduleValidator.validateBeforeSave(scheduleCellForm, errors);

        return hasErrors() ? errors() : response(OK, scheduleEditService.save(scheduleCellForm));
    }

    @PostMapping("/copy/{id}")
    protected ResponseEntity<?> saveScheduleCellAsCopy(@PathVariable UUID id, @RequestBody GroupWorkoutForm groupWorkoutForm) {

        scheduleValidator.validateBeforeSaveAsCopy(id, groupWorkoutForm, errors);

        return hasErrors() ? errors() : response(OK, scheduleEditService.saveAsCopy(id, groupWorkoutForm));
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

    @DeleteMapping("/{id}")
    protected ResponseEntity<?> deleteScheduleCell(@PathVariable UUID id) {

        scheduleValidator.validateBeforeDelete(id, errors);

        if (hasErrors()) return errors();

        scheduleEditService.delete(id);

        return response(OK);
    }
}

