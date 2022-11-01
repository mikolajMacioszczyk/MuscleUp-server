package groups.groupTrainer.trainer;

import org.springframework.http.HttpStatus;

import java.util.UUID;

public interface TrainerValidator {

    HttpStatus checkTrainerId(UUID trainerId);
}
