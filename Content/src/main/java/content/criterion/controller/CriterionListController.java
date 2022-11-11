package content.criterion.controller;

import content.common.abstracts.AbstractListController;
import content.criterion.entity.CriterionDto;
import content.criterion.repository.CriterionQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

import static org.springframework.http.HttpStatus.NOT_FOUND;
import static org.springframework.http.HttpStatus.OK;


@RestController
@RequestMapping("criterion")
public class CriterionListController extends AbstractListController {

    private final CriterionQuery criterionQuery;


    @Autowired
    private CriterionListController(CriterionQuery criterionQuery) {

        Assert.notNull(criterionQuery, "criterionQuery must not be null");

        this.criterionQuery = criterionQuery;
    }


    @GetMapping("/{id}")
    protected ResponseEntity<?> getCriterionById(@PathVariable("id") UUID id) {

        Optional<CriterionDto> criterionDto = criterionQuery.findById(id);

        return criterionDto.isPresent() ? response(OK, criterionDto.get()) : response(NOT_FOUND);
    }

    @GetMapping("/all")
    protected ResponseEntity<?> getAllCriteria() {

        List<CriterionDto> criteria = criterionQuery.getAllCriteria();

        return response(OK, criteria);
    }

    @GetMapping("/all-active")
    protected ResponseEntity<?> getAllActiveExercises() {

        List<CriterionDto> exercises = criterionQuery.getAllActiveCriteria();

        return response(OK, exercises);
    }
}