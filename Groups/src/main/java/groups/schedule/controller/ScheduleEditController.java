package groups.schedule.controller;

import groups.common.abstracts.AbstractEditController;
import groups.schedule.controller.form.ScheduleCellForm;
import groups.schedule.service.ScheduleService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("schedule")
public class ScheduleEditController extends AbstractEditController {

    private final ScheduleService scheduleService;
    private final ScheduleValidator scheduleValidator;


    @Autowired
    private ScheduleEditController(ScheduleService scheduleService) {

        Assert.notNull(scheduleService, "scheduleService must not be null");

        this.scheduleService = scheduleService;
        this.scheduleValidator = new ScheduleValidator();
    }


    @PostMapping("/create")
    protected ResponseEntity<?> createScheduleCell(@RequestBody ScheduleCellForm scheduleCellForm) {

        scheduleValidator.validateBeforeSave(scheduleCellForm, errors);

        return hasErrors() ? errors() : response(OK, "");
    }

    // TODO zmiany dla wszystkich ktore beda kopiowane, albo tylko dla pojedynczego scheduleCell
    @PutMapping("/update/{id}")
    protected ResponseEntity<?> updateScheduleCell(@PathVariable UUID id, @RequestBody ScheduleCellForm scheduleCellForm) {

        scheduleValidator.validateBeforeUpdate(id, scheduleCellForm, errors);

        return hasErrors() ? errors() : response(OK, "");
    }
}

