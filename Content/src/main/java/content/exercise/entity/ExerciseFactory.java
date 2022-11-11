package content.exercise.entity;

import content.criterion.repository.CriterionRepository;
import content.exercise.controller.form.ExerciseForm;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

@Service
public class ExerciseFactory {

    private final CriterionRepository criterionRepository;

    public ExerciseFactory(CriterionRepository criterionRepository) {

        Assert.notNull(criterionRepository, "criterionRepository must not be null");

        this.criterionRepository = criterionRepository;
    }


    public Exercise create(ExerciseForm form) {

        return new Exercise(
                form.name(),
                form.description(),
                form.imageUrl(),
                form.videoUrl(),
                true,
                criterionRepository.getByIds(form.criteria())
        );
    }
}
