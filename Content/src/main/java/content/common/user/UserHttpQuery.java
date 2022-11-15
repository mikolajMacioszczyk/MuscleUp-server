package content.common.user;

import content.common.innerCommunicators.AbstractHttpQuery;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;
import java.util.logging.Level;
import java.util.logging.Logger;

import static content.common.utils.StringUtils.concatenate;

@Service
public class UserHttpQuery extends AbstractHttpQuery implements UserQuery {

    private static final String GET_USER_BY_ID_PATH = "auth/user/by-id/";


    private final UserFactory userFactory;


    public UserHttpQuery() {

        this.userFactory = new UserFactory();
    }


    @Override
    public HttpStatus checkUserId(UUID userId) {

        Assert.notNull(userId, "userId must not be null");

        String path = concatenate(GET_USER_BY_ID_PATH, userId.toString());

        return checkIfIdExist(path);
    }

    @Override
    public User getUserById(UUID userId) {

        Assert.notNull(userId, "userId must not be null");

        String path = concatenate(GET_USER_BY_ID_PATH, userId.toString());

        String jsonUser = getById(path);

        Logger.getGlobal().log(Level.WARNING, jsonUser);

        return userFactory.create(jsonUser, userId);
    }
}
