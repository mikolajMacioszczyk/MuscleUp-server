package groups.group.fitnessClub;

import groups.common.innerCommunicators.AbstractHttpQuery;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static groups.common.utils.StringUtils.concatenate;


@Service
public class FitnessClubHttpQuery extends AbstractHttpQuery implements FitnessClubQuery {

    private static final String GET_FITNESS_CLUB_BY_ID_PATH = "fitnessclubs/fitness-club/";


    @Override
    public HttpStatus checkFitnessClubId(UUID fitnessClubId) {

        Assert.notNull(fitnessClubId, "fitnessClubId must not be null");

        String path = concatenate(GET_FITNESS_CLUB_BY_ID_PATH, fitnessClubId.toString());

        return checkIfIdExist(path);
    }
}
