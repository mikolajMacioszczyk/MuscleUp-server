package content.bodyPart.controller;

import content.bodyPart.entity.BodyPartDto;
import content.bodyPart.repository.BodyPartQuery;
import content.common.abstracts.AbstractListController;
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
@RequestMapping("body-part")
public class BodyPartListController extends AbstractListController {

    private final BodyPartQuery bodyPartQuery;


    @Autowired
    private BodyPartListController(BodyPartQuery bodyPartQuery) {

        Assert.notNull(bodyPartQuery, "bodyPartQuery must not be null");

        this.bodyPartQuery = bodyPartQuery;
    }


    @GetMapping("/{id}")
    protected ResponseEntity<?> getBodyPartById(@PathVariable("id") UUID id) {

        Optional<BodyPartDto> bodyPartDto = bodyPartQuery.findById(id);

        return bodyPartDto.isPresent() ? response(OK, bodyPartDto.get()) : response(NOT_FOUND);
    }

    @GetMapping("/all")
    protected ResponseEntity<?> getAllBodyParts() {

        List<BodyPartDto> bodyParts = bodyPartQuery.getAllBodyParts();

        return response(OK, bodyParts);
    }
}