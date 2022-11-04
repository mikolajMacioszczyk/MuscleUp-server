package groups.schedule.controller;

import groups.common.abstracts.AbstractListController;
import groups.schedule.dto.ScheduleCell;
import groups.schedule.service.ScheduleService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("schedule")
public class ScheduleListController extends AbstractListController {

    private final ScheduleService scheduleService;


    @Autowired
    private ScheduleListController(ScheduleService scheduleService) {

        Assert.notNull(scheduleService, "scheduleService must not be null");

        this.scheduleService = scheduleService;
    }


    @GetMapping("/all")
    protected ResponseEntity<?> getScheduleCells() {

        List<ScheduleCell> scheduleCells = scheduleService.composeAllCells();

        return response(OK, scheduleCells);
    }

    @GetMapping("/get/{id}")
    protected ResponseEntity<?> getScheduleCellById(@PathVariable("id") UUID id) {

        ScheduleCell scheduleCell = scheduleService.composeCell(id);

        return response(OK, scheduleCell);
    }
}